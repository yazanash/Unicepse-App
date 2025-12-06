using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class TrainerRevenueService: ITrainerRevenueService
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public TrainerRevenueService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<TrainerDueses> GetTrainerDuesAsync(Employee trainer, int year, int month)
        {
            using var context = _contextFactory.CreateDbContext();

            DateTime monthStart = new DateTime(year, month, 1);
            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);

            // جلب كل الدفعات اللي تتقاطع مع الشهر الحالي + المتأخرة
            var payments = await context.Set<PlayerPayment>()
                .Include(p => p.Player)
                .Include(p => p.Subscription) // افترضنا علاقة بين الدفعة والاشتراك
                .Where(p => p.Subscription!.TrainerId== trainer.Id && (p.CoveredFrom <= monthEnd && p.CoveredTo >= monthStart)
                         || (p.PayDate.Month == month && p.PayDate.Year == year && p.CoveredTo < monthStart))
                .AsNoTracking()
                .ToListAsync();


            var credits = await context.Set<Credit>()
                .Where(c => c.EmpPerson!.Id == trainer.Id && c.Date.Month == month && c.Date.Year == year)
                .ToListAsync();
            var result = CalculateTrainerDause(trainer, monthStart, monthEnd, payments);
            result.Credits = credits.Sum(c => c.CreditValue);
            result.CreditsCount = credits.Count();
            result.Salary = trainer.SalaryValue;
            return result;
        }

        private TrainerDueses CalculateTrainerDause(Employee trainer, DateTime monthStart, DateTime monthEnd, List<PlayerPayment> payments)
        {
            var result = new TrainerDueses
            {
                Trainer = trainer,
                Parcent = trainer.ParcentValue / 100.0,
                IssueDate = monthStart
            };

            foreach (var p in payments)
            {
                DateTime effectiveStart = p.CoveredFrom < monthStart ? monthStart : p.CoveredFrom;
                DateTime effectiveEnd = p.CoveredTo > monthEnd ? monthEnd : p.CoveredTo;

                int daysInMonth = (effectiveEnd - effectiveStart).Days + 1;
                double dailyValue = p.PaymentValue / p.CoveredDays;
                double amountForMonth = dailyValue * daysInMonth * result.Parcent;

                bool isLate = p.PayDate > p.CoveredTo;

                result.TotalSubscriptions += amountForMonth;
                result.CountSubscription++;

                result.Details.Add(new TrainerDuesDetail
                {
                    SubscriptionId = p.SubscriptionId,
                    PlayerName = p.Player?.FullName ?? "",
                    SportName = p.Subscription?.SportName ?? "",
                    PaymentValue = p.PaymentValue,
                    CoveredFrom = p.CoveredFrom,
                    CoveredTo = p.CoveredTo,
                    AmountForMonth = amountForMonth,
                    IsLatePayment = isLate
                });
            }
            return result;
        }

      
    }
}
