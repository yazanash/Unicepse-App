using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class MonthlyReportService: IMonthlyReportService
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public MonthlyReportService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<MonthlyReport> GenerateMonthlyBaseReport(int year, int month)
        {
            using var context = _contextFactory.CreateDbContext();

            DateTime monthStart = new(year, month, 1);
            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);

            // جلب دفعات الشهر
            var payments = await context.Set<PlayerPayment>()
                .Where(p => p.PayDate >= monthStart && p.PayDate <= monthEnd)
                .ToListAsync();

            // كل المصاريف
            var expenses = await context.Set<Expenses>()
                .Where(e => e.date >= monthStart && e.date <= monthEnd)
                .ToListAsync();

            double totalIncome = payments.Sum(x => x.PaymentValue);

            double incomeNextMonth = payments.Sum(p =>
            {
                if (p.CoveredTo > monthEnd)
                {
                    DateTime effectiveStart = monthEnd.AddDays(1);
                    int days = (p.CoveredTo - effectiveStart).Days + 1;
                    return (p.PaymentValue / p.CoveredDays) * days;
                }
                return 0;
            });

            double incomeFromLastMonth = payments.Sum(p =>
            {
                if (p.CoveredFrom < monthStart)
                {
                    DateTime effectiveEnd = monthStart.AddDays(-1);
                    int days = (effectiveEnd - p.CoveredFrom).Days + 1;
                    return (p.PaymentValue / p.CoveredDays) * days;
                }
                return 0;
            });

            return new MonthlyReport
            {
                TotalIncome = totalIncome,
                IncomeForNextMonth = incomeNextMonth,
                IncomeFromLastMonth = incomeFromLastMonth,
                Expenses = expenses.Sum(x => x.Value)
            };
        }
    }
}
