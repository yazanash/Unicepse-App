using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;
using Uniceps.Entityframework.Services;

namespace Uniceps.Stores
{
    public class GymStore
    {
        private readonly IDataService<Employee> _employeeDataService;
        private readonly ITrainerRevenueService _trainerRevenueService;
        private readonly IMonthlyReportService _monthlyReportService;
        private readonly ILogger<GymStore> _logger;
        public event Action<MonthlyReport>? ReportLoaded;
        public GymStore(ILogger<GymStore> logger, IDataService<Employee> employeeDataService, ITrainerRevenueService trainerRevenueService, IMonthlyReportService monthlyReportService)
        {
            _logger = logger;
            _employeeDataService = employeeDataService;
            _trainerRevenueService = trainerRevenueService;
            _monthlyReportService = monthlyReportService;
        }
        public async Task GetReport(DateTime date)
        {
            var report = await _monthlyReportService.GenerateMonthlyBaseReport(date.Year, date.Month);
            var employees = await _employeeDataService.GetAll();
            report.Salaries = employees.Sum(e => e.SalaryValue);

            var trainers = employees.Where(e => e.IsTrainer&&e.ParcentValue>0).ToList();
            double totalDues = 0;

            foreach (var t in trainers)
            {
                var dues = await _trainerRevenueService.GetTrainerDuesAsync(t, date.Year, date.Month);
                totalDues += (dues.TotalSubscriptions - dues.Credits); // مثال حساب النهائي
            }
            report.TrainerDauses = totalDues;

            report.NetIncome =
           report.TotalIncome
         - report.IncomeForNextMonth
         + report.IncomeFromLastMonth
         - report.Salaries
         - report.TrainerDauses
         - report.Expenses;
            ReportLoaded?.Invoke(report);
        }
    }
}
