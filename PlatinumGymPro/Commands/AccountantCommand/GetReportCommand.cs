using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.Accountant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.AccountantCommand
{
    public class GetReportCommand : AsyncCommandBase
    {

        private readonly GymStore _gymStore;
        private readonly MounthlyReportViewModel _mounthlyReportViewModel;
        public GetReportCommand(GymStore gymStore, MounthlyReportViewModel mounthlyReportViewModel)
        {
            _gymStore = gymStore;
            _mounthlyReportViewModel = mounthlyReportViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _gymStore.GetReport(_mounthlyReportViewModel.Date);
        }
    }
}
