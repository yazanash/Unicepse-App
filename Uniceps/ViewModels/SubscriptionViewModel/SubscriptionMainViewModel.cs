using ModalControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.SubscriptionCommand;
using Uniceps.Core.Models.Subscription;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.SportsViewModels;
using Uniceps.Views.SubscriptionView;

namespace Uniceps.ViewModels.SubscriptionViewModel
{
    public class SubscriptionMainViewModel: ListingViewModelBase
    {
        private readonly SubscriptionDataStore _dataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly SportDataStore  _sportDataStore;
        private readonly ObservableCollection<SubscriptionListItemViewModel> _subscriptionListItemViewModels;
        public IEnumerable<SubscriptionListItemViewModel> SubscriptionList => _subscriptionListItemViewModels;
        public ICommand LoadSubscriptionCommand { get; }
        public ICommand AddCommand => new RelayCommand(OpenCreateSubscription);
        public SearchBoxViewModel SearchBox { get; set; }
        public void OpenCreateSubscription()
        {
            CreateSubscriptionWindowViewModel createSubscriptionWindowViewModel = CreateSubscriptionWindowViewModel.LoadViewModel(_sportDataStore, _dataStore, _playersDataStore);
            SubscriptionCreationViewWindow subscriptionCreationViewWindow = new SubscriptionCreationViewWindow();
            subscriptionCreationViewWindow.DataContext = createSubscriptionWindowViewModel;
            subscriptionCreationViewWindow.Show();
        }
        public SubscriptionMainViewModel(SubscriptionDataStore dataStore, PlayersDataStore playersDataStore, SportDataStore sportDataStore)
        {
            _dataStore = dataStore;
            _dataStore.Loaded += _dataStore_Loaded;
            _dataStore.Created += _subscriptionStore_Created;
            _dataStore.Updated += _subscriptionStore_Updated;
            _dataStore.Deleted += _subscriptionStore_Deleted;
            SearchBox = new SearchBoxViewModel();
            _subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            LoadSubscriptionCommand = new LoadActiveSubscriptionCommand(_dataStore, this);
            _playersDataStore = playersDataStore;
            _sportDataStore = sportDataStore;
        }
        private SubscriptionListItemViewModel? _selectedSubscription;
        public SubscriptionListItemViewModel? SelectedSubscription
        {
            get
            {
                return _selectedSubscription;
            }
            set
            {
                _selectedSubscription = value;

                OnPropertyChanged(nameof(SelectedSubscription));
            }
        }
        private void _subscriptionStore_Deleted(int id)
        {
            SubscriptionListItemViewModel? itemViewModel = _subscriptionListItemViewModels.FirstOrDefault(y => y.Subscription?.Id == id);

            if (itemViewModel != null)
            {
                _subscriptionListItemViewModels.Remove(itemViewModel);
            }
        }

        private void _subscriptionStore_Updated(Subscription subscription)
        {
            SubscriptionListItemViewModel? subscriptionViewModel =
                  _subscriptionListItemViewModels.FirstOrDefault(y => y.Subscription.Id == subscription.Id);

            if (subscriptionViewModel != null)
            {
                subscriptionViewModel.Update(subscription);
            }
        }

        private void _subscriptionStore_Created(Subscription subscription)
        {
            AddSubscription(subscription);
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
        public static SubscriptionMainViewModel LoadViewModel(SubscriptionDataStore dataStore, PlayersDataStore playersDataStore, SportDataStore sportDataStore)
        {
            SubscriptionMainViewModel viewModel = new(dataStore, playersDataStore, sportDataStore);

            viewModel.LoadSubscriptionCommand.Execute(null);

            return viewModel;
        }

    }
}
