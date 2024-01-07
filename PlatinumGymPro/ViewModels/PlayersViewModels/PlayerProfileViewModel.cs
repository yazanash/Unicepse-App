using PlatinumGym.Core.Models.Player;
using PlatinumGym.Core.Models.Subscription;
using PlatinumGymPro.Commands.SubscriptionCommand;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.SubscriptionViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.PlayersViewModels
{
    public class PlayerProfileViewModel : ViewModelBase
    {
        private readonly ObservableCollection<SubscriptionListItemViewModel> subscriptionListItemViewModels;
        private NavigationStore _navigatorStore;
        private readonly SubscriptionDataStore _subscriptionStore;
        public Player _player;
        public IEnumerable<SubscriptionListItemViewModel> SubscriptionList => subscriptionListItemViewModels;


        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public ICommand LoadSubscriptionCommand { get; }
        public PlayerProfileViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionStore, Player player)
        {
            _navigatorStore = navigatorStore;
            _subscriptionStore = subscriptionStore;
            _player = player;
            LoadSubscriptionCommand = new LoadSubscriptions(this, _subscriptionStore, _player);
            subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            _subscriptionStore.Loaded += _subscriptionStore_Loaded;
            _subscriptionStore.Created += _subscriptionStore_Created;
            _subscriptionStore.Updated += _subscriptionStore_Updated;
            _subscriptionStore.Deleted += _subscriptionStore_Deleted;
        }

        private void _subscriptionStore_Deleted(int id)
        {
            SubscriptionListItemViewModel? itemViewModel = subscriptionListItemViewModels.FirstOrDefault(y => y.Subscription?.Id == id);

            if (itemViewModel != null)
            {
                subscriptionListItemViewModels.Remove(itemViewModel);
            }
        }

        private void _subscriptionStore_Updated(Subscription subscription)
        {
            SubscriptionListItemViewModel? subscriptionViewModel =
                  subscriptionListItemViewModels.FirstOrDefault(y => y.Subscription.Id == subscription.Id);

            if (subscriptionViewModel != null)
            {
                subscriptionViewModel.Update(subscription);
            }
        }

        private void _subscriptionStore_Created(Subscription subscription)
        {
            AddSubscription(subscription);
        }

        private void _subscriptionStore_Loaded()
        {
            subscriptionListItemViewModels.Clear();

            foreach (Subscription subscription in _subscriptionStore.Subscriptions)
            {
                AddSubscription(subscription);
            }
        }

        protected override void Dispose()
        {
            _subscriptionStore.Loaded -= _subscriptionStore_Loaded;
            _subscriptionStore.Created -= _subscriptionStore_Created;
            _subscriptionStore.Updated -= _subscriptionStore_Updated;
            _subscriptionStore.Deleted -= _subscriptionStore_Deleted;
            base.Dispose();
        }
        private void AddSubscription(Subscription subscription)
        {
            SubscriptionListItemViewModel itemViewModel =
                new SubscriptionListItemViewModel(subscription, _navigatorStore, _subscriptionStore);
            subscriptionListItemViewModels.Add(itemViewModel);
        }
        public static PlayerProfileViewModel LoadViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionDataStore,Player player)
        {
            PlayerProfileViewModel viewModel = new PlayerProfileViewModel(navigatorStore, subscriptionDataStore, player);
            viewModel.LoadSubscriptionCommand.Execute(null);
            return viewModel;
        }
    }
}
