using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class DashboardAnalyticsDataService : IDashboardAnalyticsDataService
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public DashboardAnalyticsDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<DashboardAnalyticsModel> GetDashboardAnalytics()
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                var today = DateTime.Today;
                var sixMonthsAgo = today.AddMonths(-6);
                var lastWeek = today.AddDays(-7);

                var model = new DashboardAnalyticsModel();

                model.TotalPaymentsToday = await context.Set<PlayerPayment>()
                    .Where(p => p.PayDate.Date == today)
                    .SumAsync(p => p.PaymentValue);

                model.TotalExpensesToday = await context.Set<Expenses>()
                    .Where(e => e.date.Date == today)
                    .SumAsync(e => e.Value);

                model.TotalCredits = await context.Set<Credit>().Where(e => e.Date.Date == today)
                    .SumAsync(e => e.CreditValue);

                model.StaffCount = await context.Set<Employee>().Where(x=>!x.IsTrainer).CountAsync();
                model.TrainersCount = await context.Set<Employee>().Where(x => x.IsTrainer).CountAsync();

                model.CurrentPresentPlayers = await context.Set<DailyPlayerReport>()
            .CountAsync(r => r.loginTime.Date == today && r.IsLogged);


                model.ActivePlayersCount = await context.Set<Player>()
                    .CountAsync(p => p.IsSubscribed);

                model.MaleCount = await context.Set<Player>().CountAsync(p => p.GenderMale);
                model.FemaleCount = await context.Set<Player>().CountAsync(p => !p.GenderMale);

                model.SportPopularity = await context.Set<Subscription>()
              .GroupBy(r => r.SportName!)
              .Select(g => new { Sport = g.Key ?? "غير محدد", Count = g.Count() })
              .ToDictionaryAsync(x => x.Sport, x => x.Count);

                var reportWithGender = await (from report in context.Set<DailyPlayerReport>()
                                              join player in context.Set<Player>() on report.PlayerId equals player.Id
                                              where report.loginTime >= lastWeek
                                              select new
                                              {
                                                  Hour = report.loginTime.Hour,
                                                  IsMale = player.GenderMale
                                              }).ToListAsync();

                model.PeakHours = reportWithGender
                    .GroupBy(x => new { x.Hour, x.IsMale })
                    .Select(g => new HourlyAttendanceDto
                    {
                        HourInt = g.Key.Hour,
                        IsMale = g.Key.IsMale,
                        Count = (int)Math.Ceiling(g.Count() / 7.0)
                    })
                    .OrderBy(x => x.HourInt)
                    .ToList();
                var weeklyDataRaw = await (from report in context.Set<DailyPlayerReport>()
                                           join player in context.Set<Player>() on report.PlayerId equals player.Id
                                           where report.loginTime >= lastWeek
                                           select new
                                           {
                                               Date = report.loginTime.Date,
                                               IsMale = player.GenderMale
                                           }).ToListAsync();

                model.WeeklyAttendance = weeklyDataRaw
                    .GroupBy(x => x.Date)
                    .Select(g => new DayAttendanceDto
                    {
                        Date = g.Key,
                        MaleCount = g.Count(x => x.IsMale),
                        FemaleCount = g.Count(x => !x.IsMale)
                    })
                    .OrderBy(x => x.Date)
                    .ToList();
                var payments = await context.Set<PlayerPayment>()
                    .Where(p => p.PayDate >= sixMonthsAgo)
                    .ToListAsync();

                var expenses = await context.Set<Expenses>()
                    .Where(e => e.date >= sixMonthsAgo)
                    .ToListAsync();
                var credits = await context.Set<Credit>()
                   .Where(e => e.Date >= sixMonthsAgo)
                   .ToListAsync();

                var financialHistory = new List<FinancialHistoryDto>();

                for (int i = 0; i <= 6; i++)
                {
                    var targetDate = sixMonthsAgo.AddMonths(i);
                    var year = targetDate.Year;
                    var month = targetDate.Month;

                    var monthlyRevenue = payments
                        .Where(p => p.PayDate.Year == year && p.PayDate.Month == month)
                        .Sum(p => p.PaymentValue);

                    var monthlyExpenses = expenses
                        .Where(e => e.date.Year == year && e.date.Month == month)
                        .Sum(e => e.Value);

                    var monthlyCredits= credits
                       .Where(e => e.Date.Year == year && e.Date.Month == month)
                       .Sum(e => e.CreditValue);

                    financialHistory.Add(new FinancialHistoryDto
                    {
                        Month = $"{month}/{year}",
                        Revenue = monthlyRevenue,
                        Expenses = monthlyExpenses + monthlyCredits
                    });
                }

                model.FinancialHistory = financialHistory;

                return model;
            }
        }
    }
}
