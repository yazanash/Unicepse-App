using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.AccountantCommand;
using Unicepse.Core.Models.Subscription;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.ViewModels.SubscriptionViewModel;

namespace Unicepse.ViewModels.Accountant
{
    public class SubscriptionCardViewModel : ListingViewModelBase
    {
        private readonly GymStore _gymStore;
        private readonly AccountingStateViewModel _accountingStateViewModel;
        public SubscriptionCardViewModel(GymStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            _gymStore = gymStore;
            _accountingStateViewModel = accountingStateViewModel;
            _incomeListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            LoadSubscriptionsCommand = new LoadDailySubscriptions(_gymStore, _accountingStateViewModel);
            _gymStore.SubscriptionsLoaded += _gymStore_PaymentsLoaded; 
        }

        private void _gymStore_PaymentsLoaded()
        {
            _incomeListItemViewModels.Clear();
            foreach (Subscription subscription in _gymStore.Subscriptions)
            {
                AddSubscription(subscription);
            }
        }

        private readonly ObservableCollection<SubscriptionListItemViewModel> _incomeListItemViewModels;
        public IEnumerable<SubscriptionListItemViewModel> SubcriptionsList => _incomeListItemViewModels;

        private void AddSubscription(Subscription  subscription)
        {
            SubscriptionListItemViewModel itemViewModel =
                 new SubscriptionListItemViewModel(subscription);
            _incomeListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = _incomeListItemViewModels.Count();
        }
        public ICommand LoadSubscriptionsCommand;
        public static SubscriptionCardViewModel LoadViewModel(GymStore gymStore,AccountingStateViewModel accountingStateViewModel)
        {
            SubscriptionCardViewModel viewModel = new SubscriptionCardViewModel(gymStore,accountingStateViewModel);

            viewModel.LoadSubscriptionsCommand.Execute(null);

            return viewModel;
        }
    }
}
