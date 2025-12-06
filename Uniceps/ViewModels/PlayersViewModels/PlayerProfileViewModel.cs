using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Commands.SubscriptionCommand;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Subscription;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.Metrics;
using Uniceps.ViewModels.PaymentsViewModels;
using Uniceps.ViewModels.PlayersAttendenceViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;

namespace Uniceps.ViewModels.PlayersViewModels
{
    public class PlayerProfileViewModel : ViewModelBase
    {
        private NavigationStore _navigatorStore;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly SportDataStore _sportDataStore;
        private readonly MetricDataStore _metricDataStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly NavigationService<PlayerListViewModel> _navigationService;
        private readonly ExercisesDataStore _exercisesDataStore;
        private readonly AccountStore _accountStore;
        private readonly EmployeeStore _employeeStore;
        private PlayerMainPageViewModel _playerMainPageViewModel;
        public PlayerListItemViewModel? Player { get; set; }
        public ViewModelBase? CurrentPlayerViewModel => _navigatorStore.CurrentViewModel;

        public PlayerProfileViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionStore,
            PlayersDataStore playersDataStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, PlayersAttendenceStore playersAttendenceStore,
            NavigationService<PlayerListViewModel> navigationService, ExercisesDataStore exercisesDataStore, AccountStore accountStore, EmployeeStore employeeStore)
        {
            _navigatorStore = new NavigationStore();
            _subscriptionStore = subscriptionStore;
            _playersDataStore = playersDataStore;
            _sportDataStore = sportDataStore;
            _paymentDataStore = paymentDataStore;
            _metricDataStore = metricDataStore;
            _playersAttendenceStore = playersAttendenceStore;
            _navigationService = navigationService;
            _exercisesDataStore = exercisesDataStore;
            _accountStore = accountStore;
            _employeeStore = employeeStore;
            _playerMainPageViewModel = LoadPlayerMainPageViewModel(_navigatorStore, _playersDataStore, _subscriptionStore, _paymentDataStore, _sportDataStore, _accountStore, _employeeStore);
            _navigatorStore.CurrentViewModel = LoadPlayerMainPageViewModel(_navigatorStore, _playersDataStore, _subscriptionStore, _paymentDataStore, _sportDataStore, _accountStore, _employeeStore);
            _navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
            _playersDataStore.PlayerChanged += _playersDataStore_PlayerChanged;
            _playersDataStore.ArchivedPlayer_restored += _playersDataStore_ArchivedPlayer_restored;
            _playersDataStore.Player_update += _playersDataStore_Player_update;
            PlayerHomeCommand = new NavaigateCommand<PlayerMainPageViewModel>(new NavigationService<PlayerMainPageViewModel>(_navigatorStore, () => _playerMainPageViewModel));
            SubscriptionCommand = new NavaigateCommand<SubscriptionDetailsViewModel>(new NavigationService<SubscriptionDetailsViewModel>(_navigatorStore, () => LoadSubscriptionViewModel(_navigatorStore, _sportDataStore, _subscriptionStore, _playersDataStore, _paymentDataStore, _playerMainPageViewModel)));
            PaymentCommand = new NavaigateCommand<PaymentListViewModel>(new NavigationService<PaymentListViewModel>(_navigatorStore, () => LoadPaymentsViewModel(_paymentDataStore, _playersDataStore, _navigatorStore, _subscriptionStore)));

            if (_accountStore.SystemSubscription != null)
            {
                MetricsCommand = new NavaigateCommand<MetricReportViewModel>(new NavigationService<MetricReportViewModel>(_navigatorStore, () => LoadMetricsViewModel(_metricDataStore, _playersDataStore, _navigatorStore)));
                PlayerAttendenceCommand = new NavaigateCommand<PlayerAttendenceViewModel>(new NavigationService<PlayerAttendenceViewModel>(_navigatorStore, () => LoadPlayerAttendenceViewModel(_playersAttendenceStore, _playersDataStore)));

            }
            else
            {
                MetricsCommand = new NavaigateCommand<PremiumViewModel>(new NavigationService<PremiumViewModel>(_navigatorStore, () => new PremiumViewModel()));
                PlayerAttendenceCommand = new NavaigateCommand<PremiumViewModel>(new NavigationService<PremiumViewModel>(_navigatorStore, () => new PremiumViewModel()));

            }
        }

        private void _playersDataStore_Player_update(Player obj)
        {
            if (Player != null && Player.Player.Id == obj.Id)
                Player.Update(obj);
        }

        private void _playersDataStore_ArchivedPlayer_restored(Player obj)
        {
            if (Player != null && Player.Player.Id == obj.Id)
                Player.Update(obj);
        }

        private PaymentListViewModel LoadPaymentsViewModel(PaymentDataStore paymentDataStore, PlayersDataStore playersDataStore, NavigationStore navigatorStore, SubscriptionDataStore subscriptionDataStore)
        {
            return PaymentListViewModel.LoadViewModel(paymentDataStore, playersDataStore, navigatorStore, subscriptionDataStore);
        }

        private void _playersDataStore_PlayerChanged(Player? obj)
        {
            _playerMainPageViewModel = LoadPlayerMainPageViewModel(_navigatorStore, _playersDataStore, _subscriptionStore, _paymentDataStore, _sportDataStore, _accountStore,_employeeStore);
            Player = new PlayerListItemViewModel(_playersDataStore.SelectedPlayer!, _navigatorStore, _playersDataStore,
               _navigationService, _playerMainPageViewModel);
            _navigatorStore.CurrentViewModel = _playerMainPageViewModel;
            OnPropertyChanged(nameof(Player));
        }

        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentPlayerViewModel));
        }
        private PlayerMainPageViewModel LoadPlayerMainPageViewModel(NavigationStore navigatorStore, PlayersDataStore playerStore, SubscriptionDataStore subscriptionDataStore, PaymentDataStore paymentDataStore, SportDataStore sportDataStore,AccountStore accountStore,EmployeeStore employeeStore)
        {
            return PlayerMainPageViewModel.LoadViewModel(navigatorStore, subscriptionDataStore, playerStore, paymentDataStore, sportDataStore, accountStore, employeeStore);
        }

        private SubscriptionDetailsViewModel LoadSubscriptionViewModel(NavigationStore navigatorStore, SportDataStore sportDataStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, PaymentDataStore paymentDataStore, PlayerMainPageViewModel playerMainPageViewModel)
        {
            return SubscriptionDetailsViewModel.LoadViewModel(sportDataStore, navigatorStore, subscriptionDataStore, playersDataStore, paymentDataStore, playerMainPageViewModel);
        }
        private AddPaymentViewModel LoadAddPaymentViewModel(PaymentDataStore paymentDataStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, NavigationStore navigatorStore, PaymentListViewModel paymentListViewModel)
        {
            return AddPaymentViewModel.LoadViewModel(paymentDataStore, subscriptionDataStore, playersDataStore, navigatorStore, paymentListViewModel);
        }
        private MetricReportViewModel LoadMetricsViewModel(MetricDataStore metricDataStore, PlayersDataStore playerDataStore, NavigationStore navigationStore)
        {
            return MetricReportViewModel.LoadViewModel(metricDataStore, playerDataStore, navigationStore);
        }

        private PlayerAttendenceViewModel LoadPlayerAttendenceViewModel(PlayersAttendenceStore playersAttendenceStore, PlayersDataStore playerDataStore)
        {
            return PlayerAttendenceViewModel.LoadViewModel(playersAttendenceStore, playerDataStore);
        }
        public ICommand? PlayerHomeCommand { get; }
        public ICommand? SubscriptionCommand { get; }
        public ICommand? PaymentCommand { get; }
        public ICommand? MetricsCommand { get; }
        public ICommand? TrainingProgramCommand { get; }
        public ICommand? PlayerAttendenceCommand { get; }
    }
}
