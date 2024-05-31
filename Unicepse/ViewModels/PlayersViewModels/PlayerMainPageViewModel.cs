using Unicepse.Core.Models.Subscription;
using Unicepse.Commands.Payments;
using Unicepse.Commands.SubscriptionCommand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Stores;
using Unicepse.ViewModels;
using Unicepse.ViewModels.SubscriptionViewModel;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.PlayersViewModels
{
    public class PlayerMainPageViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<SubscriptionListItemViewModel> subscriptionListItemViewModels;
        private NavigationStore _navigatorStore;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PaymentDataStore _paymentStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly SportDataStore _sportDataStore;
        public PlayerListItemViewModel? Player => _playersDataStore.SelectedPlayer;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public IEnumerable<SubscriptionListItemViewModel> SubscriptionList => subscriptionListItemViewModels;
        public SubscriptionListItemViewModel? SelectedSubscription
        {
            get
            {
                return SubscriptionList
                    .FirstOrDefault(y => y?.Subscription == _subscriptionStore.SelectedSubscription);
            }
            set
            {
                _subscriptionStore.SelectedSubscription = value?.Subscription;

            }
        }

        private PlayerStatesViewModel? _playerSubscription;
        public PlayerStatesViewModel? PlayerSubscription
        {
            get
            {
                return _playerSubscription;
            }
            set
            {
                _playerSubscription = value;
                OnPropertyChanged(nameof(PlayerSubscription));
            }
        }
        private PlayerStatesViewModel? _playerPayments;
        public PlayerStatesViewModel? PlayerPayments
        {
            get
            {
                return _playerPayments;
            }
            set
            {
                _playerPayments = value;
                OnPropertyChanged(nameof(PlayerPayments));
            }
        }
        private PlayerStatesViewModel? _playerSubscriptionCount;
        public PlayerStatesViewModel? PlayerSubscriptionCount
        {
            get
            {
                return _playerSubscriptionCount;
            }
            set
            {
                _playerSubscriptionCount = value;
                OnPropertyChanged(nameof(PlayerSubscriptionCount));
            }
        }
        public PlayerMainPageViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionStore, PlayersDataStore playersDataStore, PaymentDataStore paymentStore, SportDataStore sportDataStore)
        {
            _navigatorStore = navigatorStore;
            _subscriptionStore = subscriptionStore;
            _playersDataStore = playersDataStore;
            _paymentStore = paymentStore;
            LoadSubscriptionCommand = new LoadSubscriptions(this, _subscriptionStore, _playersDataStore.SelectedPlayer!);
            LoadPaymentCommand = new LoadPaymentsCommand(_playersDataStore, _paymentStore);
            subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            _subscriptionStore.Loaded += _subscriptionStore_Loaded;
            _subscriptionStore.Created += _subscriptionStore_Created;
            _subscriptionStore.Updated += _subscriptionStore_Updated;
            _subscriptionStore.Deleted += _subscriptionStore_Deleted;
            _paymentStore.SumUpdated += _paymentStore_SumUpdated;
            PlayerSubscription = new() { PlayerState = "قيمة الاشتراكات", StateValue = 0, IconPacks = MahApps.Metro.IconPacks.PackIconMaterialKind.Account };
            PlayerPayments = new() { PlayerState = "المدفوعات", StateValue = 0, IconPacks = MahApps.Metro.IconPacks.PackIconMaterialKind.ChartBar };
            PlayerSubscriptionCount = new() { PlayerState = "الاشتراكات", StateValue = 0, IconPacks = MahApps.Metro.IconPacks.PackIconMaterialKind.AccountCashOutline };
            _sportDataStore = sportDataStore;
        }

        private void _paymentStore_SumUpdated()
        {
            PlayerPayments!.StateValue = _paymentStore.GetSum();
        }


        public ICommand LoadSubscriptionCommand { get; }
        public ICommand LoadPaymentCommand { get; }


        private void _subscriptionStore_Deleted(int id)
        {
            SubscriptionListItemViewModel? itemViewModel = subscriptionListItemViewModels.FirstOrDefault(y => y.Subscription?.Id == id);

            if (itemViewModel != null)
            {
                subscriptionListItemViewModels.Remove(itemViewModel);
                UpdateSubscriptionStatet();
            }
        }

        private void _subscriptionStore_Updated(Subscription subscription)
        {
            SubscriptionListItemViewModel? subscriptionViewModel =
                  subscriptionListItemViewModels.FirstOrDefault(y => y.Subscription.Id == subscription.Id);

            if (subscriptionViewModel != null)
            {
                subscriptionViewModel.Update(subscription);
                UpdateSubscriptionStatet();
            }
        }

        private void _subscriptionStore_Created(Subscription subscription)
        {
            AddSubscription(subscription);
            UpdateSubscriptionStatet();
        }

        private void _subscriptionStore_Loaded()
        {
            subscriptionListItemViewModels.Clear();

            foreach (Subscription subscription in _subscriptionStore.Subscriptions)
            {
                AddSubscription(subscription);
            }
            UpdateSubscriptionStatet();
        }
        private void UpdateSubscriptionStatet()
        {
            PlayerSubscriptionCount!.StateValue = subscriptionListItemViewModels.Count();
            PlayerSubscription!.StateValue = subscriptionListItemViewModels.Sum(x => x.PriceAfterOffer);
        }
        public override void Dispose()
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
                new SubscriptionListItemViewModel(subscription, _navigatorStore, _subscriptionStore, _sportDataStore, _playersDataStore, this, _paymentStore);
            subscriptionListItemViewModels.Add(itemViewModel);
        }

        public static PlayerMainPageViewModel LoadViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, PaymentDataStore paymentDataStore, SportDataStore sportDataStore)
        {
            PlayerMainPageViewModel viewModel = new PlayerMainPageViewModel(navigatorStore, subscriptionDataStore, playersDataStore, paymentDataStore, sportDataStore);
            viewModel.LoadSubscriptionCommand.Execute(null);
            viewModel.LoadPaymentCommand.Execute(null);
            return viewModel;
        }
    }
}
