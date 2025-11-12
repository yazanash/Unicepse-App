using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.SubscriptionCommand;
using Uniceps.Core.Models.Subscription;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels.PlayersViewModels;

namespace Uniceps.ViewModels.SubscriptionViewModel
{
    public class SubscriptionMainViewModel: ListingViewModelBase
    {
        private readonly SubscriptionDataStore _dataStore;
        private readonly ObservableCollection<SubscriptionListItemViewModel> _subscriptionListItemViewModels;
        public IEnumerable<SubscriptionListItemViewModel> SubscriptionList => _subscriptionListItemViewModels;
        public ICommand LoadSubscriptionCommand { get; }
        public SubscriptionMainViewModel(SubscriptionDataStore dataStore)
        {
            _dataStore = dataStore;
            _dataStore.Loaded += _dataStore_Loaded;
            _subscriptionListItemViewModels =new ObservableCollection<SubscriptionListItemViewModel>();
            LoadSubscriptionCommand = new LoadActiveSubscriptionCommand(_dataStore,this);
        }

        private void _dataStore_Loaded()
        {
            _subscriptionListItemViewModels.Clear();
           foreach (Subscription subscription in _dataStore.Subscriptions)
            {
                AddSubscription(subscription);
            }
        }
        private void AddSubscription(Subscription subscription)
        {
            SubscriptionListItemViewModel itemViewModel =
                new SubscriptionListItemViewModel(subscription);
            _subscriptionListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = _subscriptionListItemViewModels.Count();
        }
        public static SubscriptionMainViewModel LoadViewModel(SubscriptionDataStore dataStore)
        {
            SubscriptionMainViewModel viewModel = new(dataStore);

            viewModel.LoadSubscriptionCommand.Execute(null);

            return viewModel;
        }

    }
}
