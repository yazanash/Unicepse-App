using Microsoft.EntityFrameworkCore;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class TrainerRevenueService : ITrainerRevenueService
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public TrainerRevenueService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<TrainerDueses> GetTrainerDuesAsync(int empId, DateTime reportDate)
        {
            using var context = _contextFactory.CreateDbContext();
            var trainer = await context.Set<Employee>().FindAsync(empId);
            if (trainer == null) throw new Exception("هذا الموظف غير موجود");
            if (reportDate < (trainer.LastClosingDate ?? trainer.StartDate))
            {
                throw new Exception("التاريخ المدخل يقع ضمن فترة مالية مغلقة ومؤرشفة.");
            }
            DateTime startDate = trainer.LastClosingDate ?? trainer.StartDate;
            DateTime reportDateEnd = reportDate.Date.AddDays(1).AddTicks(-1);
            var payments = await context.Set<PlayerPayment>()
                .Include(p => p.Player)
                .Include(p => p.Subscription)
                .Where(p => p.Subscription!.TrainerId == trainer.Id && p.PayDate.Date > startDate.Date
             && p.PayDate.Date <= reportDate.Date)
                .AsNoTracking()
                .ToListAsync();

            var credits = await context.Set<Credit>()
                .Where(c => c.EmpPersonId == trainer.Id && c.Date > startDate      
             && c.Date <= reportDateEnd)
                .ToListAsync();

            var result = CalculateTrainerDause(trainer, startDate, reportDateEnd, payments);

            result.BalanceForward = trainer.LastClosingBalance;
            result.Credits = credits.Sum(c => c.CreditValue);  
            result.CreditsCount = credits.Count;
            result.CreditDetails = credits;
            result.TotalSubscriptions = Math.Round(result.TotalSubscriptions / 100) * 100;

            return result;
        }

        private TrainerDueses CalculateTrainerDause(Employee trainer, DateTime startDate, DateTime reportDate, List<PlayerPayment> payments)
        {
            var result = new TrainerDueses
            {
                Trainer = trainer,
                Parcent = trainer.ParcentValue / 100.0,
                IssueDate = reportDate,
                SalaryDetails = new List<SalaryDetail>()
            };

            int totalDaysSinceClosing = (reportDate - startDate).Days;
            if (totalDaysSinceClosing < 0) totalDaysSinceClosing = 0;

            double dailySalary = trainer.SalaryValue / 30;
            result.Salary = trainer.SalaryValue;

            int remainingDays = totalDaysSinceClosing;
            int monthCounter = 1;

            while (remainingDays > 0)
            {
                var salaryItem = new SalaryDetail
                {
                    BaseSalary = trainer.SalaryValue,
                    EarnedAmount = trainer.SalaryValue 
                };

                if (remainingDays >= 30)
                {
                    salaryItem.MonthName = $"الراتب المستحق - الفترة {monthCounter}";
                    salaryItem.ActualDue = trainer.SalaryValue; 
                    salaryItem.Note = "شهر مكتمل";

                    result.Salaries += trainer.SalaryValue;
                    result.TotalSalaryDebt += trainer.SalaryValue;
                    remainingDays -= 30;
                }
                else
                {
                    salaryItem.MonthName = $"الراتب المستحق - الفترة {monthCounter} (جاري)";

                    double partialDue = Math.Round(dailySalary * remainingDays);
                    salaryItem.ActualDue = partialDue;

                    salaryItem.Note = $"شهر غير مكتمل ({remainingDays} يوم)";

                    result.Salaries += trainer.SalaryValue;
                    result.TotalSalaryDebt += partialDue;
                    remainingDays = 0;
                }

                result.SalaryDetails.Add(salaryItem);
                monthCounter++;
            }

            foreach (var p in payments)
            {
                double totalTrainerShare = p.PaymentValue * result.Parcent;
                int daysPassed = (reportDate >= p.CoveredTo)
                    ? p.CoveredDays
                    : (reportDate.Date - p.CoveredFrom.Date).Days + 1;

                if (daysPassed < 0) daysPassed = 0;
                if (daysPassed > p.CoveredDays) daysPassed = p.CoveredDays;

                double dailyValue =(double) p.PaymentValue / p.CoveredDays;
                double earnedUntilReportDate = (dailyValue * daysPassed) * result.Parcent;

                result.TotalSubscriptions += totalTrainerShare; 
                result.CountSubscription++;

                result.Details.Add(new TrainerDuesDetail
                {
                    SubscriptionId = p.SubscriptionId,
                    PlayerName = p.Player?.FullName,
                    SportName = p.Subscription?.SportName,
                    PaymentValue = p.PaymentValue,
                    CoveredFrom = p.CoveredFrom,
                    CoveredTo = p.CoveredTo,
                    AmountForMonth = totalTrainerShare,
                    EarnedUntilNow = Math.Round(earnedUntilReportDate),
                    IsLatePayment = p.PayDate > p.CoveredTo
                });
            }

            return result;
        }


        public async Task<double> GetTrainersAndEmployeesCredits(int year, int month)
        {
            using var context = _contextFactory.CreateDbContext();
            var credits = await context.Set<Credit>()
                 .Where(c => c.Date.Month == month && c.Date.Year == year)
                 .ToListAsync();
            return credits.Sum(c => c.CreditValue);
        }
        public async Task<bool> CloseTrainerAccountAsync(TrainerDueses finalDues)
        {
            if (finalDues?.Trainer == null) return false;
            using var context = _contextFactory.CreateDbContext();
            using (var transaction = context.Database.BeginTransaction()) 
            {
                try
                {
                    var closingEntry = new EmployeeAccountClosing
                    {
                        EmployeeId = finalDues.Trainer.Id,
                        ClosingDate = finalDues.IssueDate,
                        ReportDate = finalDues.IssueDate,
                        BalanceForwarded = finalDues.FinalBalance,
                        TotalSalaries = finalDues.Salaries,
                        TotalCommissions = finalDues.TotalSubscriptions,
                        TotalCredits = finalDues.Credits,
                        Note = $"تصفية حساب للمدرب {finalDues.Trainer.FullName} حتى تاريخ {finalDues.IssueDate:yyyy/MM/dd}"
                    };

                    context.Set<EmployeeAccountClosing>().Add(closingEntry);

                    var employee = await context.Set<Employee>().FindAsync(finalDues.Trainer.Id);
                    if (employee != null)
                    {
                        employee.LastClosingDate = finalDues.IssueDate;
                        employee.LastClosingBalance = finalDues.FinalBalance;
                        context.Set<Employee>().Update(employee);
                    }

                    await context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

    }
}
