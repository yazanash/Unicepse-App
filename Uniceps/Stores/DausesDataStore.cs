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
using Uniceps.Services;

namespace Uniceps.Stores
{
    public class DausesDataStore
    {
        private readonly ITrainerRevenueService _trainerRevenueService;
        public TrainerDueses? MonthlyTrainerDause { get; set; }
       private readonly TrainerDuesExcelService _excelService;
        public event Action<TrainerDueses?>? StateChanged;
        public event Action<bool>? Closed;
        public DausesDataStore(ITrainerRevenueService trainerRevenueService, TrainerDuesExcelService excelService)
        {
            _trainerRevenueService = trainerRevenueService;
            _excelService = excelService;
        }
        public async Task GetMonthlyReport(Employee trainer,DateTime reportDate)
        {
            MonthlyTrainerDause = await _trainerRevenueService.GetTrainerDuesAsync(trainer, reportDate);
              StateChanged?.Invoke(MonthlyTrainerDause);
        }
        public async Task CloseTrainerAccountAsync()
        {
            if (MonthlyTrainerDause == null)
            {
                Closed?.Invoke(false);
                return;
            }
            bool closed = await _trainerRevenueService.CloseTrainerAccountAsync(MonthlyTrainerDause);
            Closed?.Invoke(closed);
        }
        public async Task ExportMonthlyReport(Employee trainer,string filePath, DateTime reportDate)
        {
            MonthlyTrainerDause = await _trainerRevenueService.GetTrainerDuesAsync(trainer, reportDate);
            _excelService.ExportToExcel(filePath, MonthlyTrainerDause);
        }
    }
}
