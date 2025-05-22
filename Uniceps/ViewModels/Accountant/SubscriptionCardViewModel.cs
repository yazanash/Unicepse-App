using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Core.Models.Subscription;
using Uniceps.Stores.AccountantStores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Commands.AccountantCommand;
using Uniceps.Stores;
using Uniceps.utlis.common;

namespace Uniceps.ViewModels.Accountant
{
    public class SubscriptionCardViewModel : ListingViewModelBase
    {
        private readonly SubscriptionDailyAccountantDataStore _gymStore;
        private readonly AccountingStateViewModel _accountingStateViewModel;
        public SubscriptionCardViewModel(SubscriptionDailyAccountantDataStore gymStore, AccountingStateViewModel accountingStateViewModel)
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

        private void AddSubscription(Subscription subscription)
        {
            SubscriptionListItemViewModel itemViewModel =
                 new SubscriptionListItemViewModel(subscription);
            _incomeListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = _incomeListItemViewModels.Count();
        }
        public ICommand LoadSubscriptionsCommand;
        public static SubscriptionCardViewModel LoadViewModel(SubscriptionDailyAccountantDataStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            SubscriptionCardViewModel viewModel = new SubscriptionCardViewModel(gymStore, accountingStateViewModel);

            viewModel.LoadSubscriptionsCommand.Execute(null);

            return viewModel;
        }
    }
}
