using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Stores.SystemAuthStores;
using Uniceps.ViewModels.SystemAuthViewModels;

namespace Uniceps.Commands.SystemAuthCommands
{
    internal class CreateSystemSubscriptionCommand : AsyncCommandBase
    {
        private readonly SystemSubscriptionStore _systemSubscriptionStore;
        private readonly SystemSubscriptionCreationViewModel _systemSubscriptionCreationViewModel;

        public CreateSystemSubscriptionCommand(SystemSubscriptionStore systemSubscriptionStore, SystemSubscriptionCreationViewModel systemSubscriptionCreationViewModel)
        {
            _systemSubscriptionStore = systemSubscriptionStore;
            _systemSubscriptionCreationViewModel = systemSubscriptionCreationViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_systemSubscriptionCreationViewModel.SelectedPlan != null)
                await _systemSubscriptionStore.Add(_systemSubscriptionCreationViewModel.SelectedPlan.SystemPlanItem);
        }
    }
}
