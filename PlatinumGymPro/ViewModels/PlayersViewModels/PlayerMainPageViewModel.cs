using PlatinumGym.Core.Models.Subscription;
using PlatinumGymPro.Commands.Payments;
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
    public class PlayerMainPageViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<SubscriptionListItemViewModel> subscriptionListItemViewModels;
        private NavigationStore _navigatorStore;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PaymentDataStore _paymentStore;
        private readonly PlayersDataStore _playersDataStore;
        public PlayerListItemViewModel? Player => _playersDataStore.SelectedPlayer;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public IEnumerable<SubscriptionListItemViewModel> SubscriptionList => subscriptionListItemViewModels;

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
        public PlayerMainPageViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionStore, PlayersDataStore playersDataStore, PaymentDataStore paymentStore)
        {
            _navigatorStore = navigatorStore;
            _subscriptionStore = subscriptionStore;
            _playersDataStore = playersDataStore;
            _paymentStore = paymentStore;
            LoadSubscriptionCommand = new LoadSubscriptions(this, _subscriptionStore, _playersDataStore.SelectedPlayer!);
            subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            _subscriptionStore.Loaded += _subscriptionStore_Loaded;
            _subscriptionStore.Created += _subscriptionStore_Created;
            _subscriptionStore.Updated += _subscriptionStore_Updated;
            _subscriptionStore.Deleted += _subscriptionStore_Deleted;
            _paymentStore.SumUpdated += _paymentStore_SumUpdated;
            LoadPaymentsCommand = new LoadPaymentsCommand(_playersDataStore.SelectedPlayer!,_paymentStore);
            PlayerSubscription = new() { PlayerState = "قيمة الاشتراكات", StateValue = 0, IconPacks = MahApps.Metro.IconPacks.PackIconMaterialKind.Account };
            PlayerPayments = new() { PlayerState = "المدفوعات", StateValue = 0, IconPacks = MahApps.Metro.IconPacks.PackIconMaterialKind.ChartBar };
            PlayerSubscriptionCount = new() { PlayerState = "الاشتراكات", StateValue = 0, IconPacks = MahApps.Metro.IconPacks.PackIconMaterialKind.AccountCashOutline };
          
        }

        private void _paymentStore_SumUpdated()
        {
            PlayerPayments!.StateValue = _paymentStore.GetSum();
        }

      
        public ICommand LoadSubscriptionCommand { get; }

        public ICommand LoadPaymentsCommand { get; }



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
            PlayerSubscription!.StateValue = subscriptionListItemViewModels.Sum(x=>x.PriceAfterOffer);
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

        public static PlayerMainPageViewModel LoadViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, PaymentDataStore paymentDataStore)
        {
            PlayerMainPageViewModel viewModel = new PlayerMainPageViewModel(navigatorStore, subscriptionDataStore, playersDataStore, paymentDataStore);
            viewModel.LoadSubscriptionCommand.Execute(null);
            viewModel.LoadPaymentsCommand.Execute(null);
            return viewModel;
        }
    }
}
