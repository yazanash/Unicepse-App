using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.Accountant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.WPF.Commands.AccountantCommand
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
