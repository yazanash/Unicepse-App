using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
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
using Uniceps.Entityframework.Services;

namespace Uniceps.Stores
{
    public class DausesDataStore
    {
        public List<Subscription> _subscriptions;
        public IEnumerable<Subscription> Subscriptions => _subscriptions;
        private readonly ITrainerRevenueService _trainerRevenueService;
        public TrainerDueses? MonthlyTrainerDause { get; set; }
        public event Action<TrainerDueses?>? StateChanged;
        public event Action<TrainerDueses?>? ReportLoaded;
        public DausesDataStore( ITrainerRevenueService trainerRevenueService)
        {
            _subscriptions = new List<Subscription>();
            _trainerRevenueService = trainerRevenueService;
        }
        public async Task GetMonthlyReport(Employee trainer, DateTime date)
        {

            MonthlyTrainerDause = await _trainerRevenueService.GetTrainerDuesAsync(trainer, date.Year,date.Month);
              StateChanged?.Invoke(MonthlyTrainerDause);
        }
        public async Task GetPrintedMonthlyReport(Employee trainer, DateTime date)
        {
            MonthlyTrainerDause = await _trainerRevenueService.GetTrainerDuesAsync(trainer, date.Year, date.Month);
            ReportLoaded?.Invoke(MonthlyTrainerDause);
        }
    }
}
