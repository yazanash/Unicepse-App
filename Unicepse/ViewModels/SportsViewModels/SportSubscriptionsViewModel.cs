using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Sport;
using Unicepse.Core.Models.Subscription;
using Unicepse.Stores;
using Unicepse.ViewModels.SubscriptionViewModel;

namespace Unicepse.ViewModels.SportsViewModels
{
    public class SportSubscriptionsViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<SubscriptionListItemViewModel> _subscriptionListItemViewModels;
        private readonly SportDataStore _sportDataStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        public IEnumerable<SubscriptionListItemViewModel> SubscriptionList => _subscriptionListItemViewModels;
        public SportSubscriptionsViewModel(SportDataStore sportDataStore, SubscriptionDataStore subscriptionDataStore)
        {
            _sportDataStore = sportDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            _subscriptionDataStore.Loaded += _subscriptionDataStore_Loaded;
            LoadSubscriptionsForSport = new LoadSubscriptionsForSport(_sportDataStore, _subscriptionDataStore);
        }

        private void _subscriptionDataStore_Loaded()
        {
            _subscriptionListItemViewModels.Clear();
            foreach (var subs in _subscriptionDataStore.Subscriptions)
                AddSubscription(subs);
        }
        private void AddSubscription(Subscription subscription)
        {
            SubscriptionListItemViewModel subscriptionListItemViewModel = new(subscription);
            _subscriptionListItemViewModels.Add(subscriptionListItemViewModel);
            subscriptionListItemViewModel.Order = _subscriptionListItemViewModels.Count();
        }
        public ICommand LoadSubscriptionsForSport { get; }
        public static SportSubscriptionsViewModel LoadViewModel(SportDataStore sportDataStore, SubscriptionDataStore subscriptionDataStore)
        {
            SportSubscriptionsViewModel viewModel = new SportSubscriptionsViewModel(sportDataStore, subscriptionDataStore);

            viewModel.LoadSubscriptionsForSport.Execute(null);

            return viewModel;
        }
    }
}
