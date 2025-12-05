using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Core.Models;
using Uniceps.navigation;
using Uniceps.Stores.SystemAuthStores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.ViewModels.SystemAuthViewModels;
using Uniceps.Views;

namespace Uniceps.Commands.SystemAuthCommands
{
    internal class CreateSystemSubscriptionCommand : AsyncCommandBase
    {
        private readonly SystemSubscriptionStore _systemSubscriptionStore;
        private readonly SystemSubscriptionCreationViewModel _systemSubscriptionCreationViewModel;
        private NavigationService<SubscriptionMainViewModel> _navigationService;

        public CreateSystemSubscriptionCommand(SystemSubscriptionStore systemSubscriptionStore, SystemSubscriptionCreationViewModel systemSubscriptionCreationViewModel, NavigationService<SubscriptionMainViewModel> navigationService)
        {
            _systemSubscriptionStore = systemSubscriptionStore;
            _systemSubscriptionCreationViewModel = systemSubscriptionCreationViewModel;
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_systemSubscriptionCreationViewModel.SelectedPlan != null)
            {
                MembershipPayment result = await _systemSubscriptionStore.Add(_systemSubscriptionCreationViewModel.SelectedPlan.SystemPlanItem);
                if (!result.RequirePayment)
                {
                    MessageBox.Show("تم شراء الاشتراك ... لا حاجة لدفع");
                    _navigationService.ReNavigate();
                }
                else
                {
                    SystemSubscriptionPaymentOptionDialogViewModel dialogViewModel = new SystemSubscriptionPaymentOptionDialogViewModel(result.PaymentUrl!, result.CashPaymentUrl??"https://t.me/uniceps_bot");
                     SystemSubscriptionPaymentOptionDialog systemSubscriptionPaymentOptionDialog = new SystemSubscriptionPaymentOptionDialog();
                    systemSubscriptionPaymentOptionDialog.DataContext = dialogViewModel;
                    systemSubscriptionPaymentOptionDialog.ShowDialog();
                    
                }
             
            }
        }

    }
}
