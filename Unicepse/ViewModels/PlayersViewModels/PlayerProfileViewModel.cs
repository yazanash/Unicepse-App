using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Subscription;
using Unicepse.Commands;
using Unicepse.Commands.SubscriptionCommand;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Stores;
using Unicepse.ViewModels.PlayersAttendenceViewModels;
using Unicepse.ViewModels.Metrics;
using Unicepse.ViewModels.PaymentsViewModels;
using Unicepse.utlis.common;
using Unicepse.ViewModels.SubscriptionViewModel;
using Unicepse.navigation.Stores;
using Unicepse.ViewModels.RoutineViewModels;

namespace Unicepse.ViewModels.PlayersViewModels
{
    public class PlayerProfileViewModel : ViewModelBase
    {
        private NavigationStore _navigatorStore;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly SportDataStore _sportDataStore;
        private readonly MetricDataStore _metricDataStore;
        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly LicenseDataStore _licenseDataStore;
        private readonly NavigationService<PlayerListViewModel> _navigationService;
        public PlayerListItemViewModel? Player { get; set; }
        public ViewModelBase? CurrentPlayerViewModel => _navigatorStore.CurrentViewModel;

        public PlayerProfileViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionStore,
            PlayersDataStore playersDataStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore, LicenseDataStore licenseDataStore,
            NavigationService<PlayerListViewModel> navigationService)
        {
            _navigatorStore = navigatorStore;
            _subscriptionStore = subscriptionStore;
            _playersDataStore = playersDataStore;
            _sportDataStore = sportDataStore;
            _paymentDataStore = paymentDataStore;
            _routineDataStore = routineDataStore;
            _metricDataStore = metricDataStore;
            _playersAttendenceStore = playersAttendenceStore;
            _licenseDataStore = licenseDataStore;
            _navigationService = navigationService;
            Player = new PlayerListItemViewModel(_playersDataStore.SelectedPlayer!,_navigatorStore,_subscriptionStore,_playersDataStore,
                _sportDataStore,_paymentDataStore,_metricDataStore,_routineDataStore,_playersAttendenceStore,_licenseDataStore,_navigationService);

            PlayerMainPageViewModel playerMainPageViewModel = LoadPlayerMainPageViewModel(_navigatorStore, _playersDataStore, _subscriptionStore, _paymentDataStore, _sportDataStore,_licenseDataStore);

            navigatorStore.CurrentViewModel = LoadPlayerMainPageViewModel(_navigatorStore, _playersDataStore, _subscriptionStore, _paymentDataStore, _sportDataStore,_licenseDataStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
            _playersDataStore.PlayerChanged += _playersDataStore_PlayerChanged;
            _playersDataStore.ArchivedPlayer_restored += _playersDataStore_ArchivedPlayer_restored;
            _playersDataStore.Player_update += _playersDataStore_Player_update;
            PlayerHomeCommand = new NavaigateCommand<PlayerMainPageViewModel>(new NavigationService<PlayerMainPageViewModel>(_navigatorStore, () => playerMainPageViewModel));
            SubscriptionCommand = new NavaigateCommand<SubscriptionDetailsViewModel>(new NavigationService<SubscriptionDetailsViewModel>(_navigatorStore, () => LoadSubscriptionViewModel(_navigatorStore, _sportDataStore, _subscriptionStore, _playersDataStore, _paymentDataStore, playerMainPageViewModel)));
            PaymentCommand = new NavaigateCommand<PaymentListViewModel>(new NavigationService<PaymentListViewModel>(_navigatorStore, () => LoadPaymentsViewModel(_paymentDataStore, _playersDataStore, _navigatorStore, _subscriptionStore)));
            MetricsCommand = new NavaigateCommand<MetricReportViewModel>(new NavigationService<MetricReportViewModel>(_navigatorStore, () => LoadMetricsViewModel(_metricDataStore, _playersDataStore, _navigatorStore)));
            TrainingProgramCommand = new NavaigateCommand<RoutinePlayerViewModels>(new NavigationService<RoutinePlayerViewModels>(_navigatorStore, () => LoadRoutineViewModel(_routineDataStore, _playersDataStore, _navigatorStore,_licenseDataStore)));
            PlayerAttendenceCommand = new NavaigateCommand<PlayerAttendenceViewModel>(new NavigationService<PlayerAttendenceViewModel>(_navigatorStore, () => LoadPlayerAttendenceViewModel(_playersAttendenceStore, _playersDataStore)));
            
        }

        private void _playersDataStore_Player_update(Player obj)
        {
            if (Player != null&&Player.Player.Id==obj.Id)
                Player.Update(obj);
        }

        private void _playersDataStore_ArchivedPlayer_restored(Player obj)
        {
            if(Player!=null && Player.Player.Id == obj.Id)
            Player.Update(obj);
        }

        private PaymentListViewModel LoadPaymentsViewModel(PaymentDataStore paymentDataStore, PlayersDataStore playersDataStore, NavigationStore navigatorStore, SubscriptionDataStore subscriptionDataStore)
        {
            return PaymentListViewModel.LoadViewModel(paymentDataStore, playersDataStore, navigatorStore, subscriptionDataStore);
        }

        private void _playersDataStore_PlayerChanged(Player? obj)
        {
            OnPropertyChanged(nameof(Player));
        }

        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentPlayerViewModel));
        }
        private PlayerMainPageViewModel LoadPlayerMainPageViewModel(NavigationStore navigatorStore, PlayersDataStore playerStore, SubscriptionDataStore subscriptionDataStore, PaymentDataStore paymentDataStore, SportDataStore sportDataStore,LicenseDataStore licenseDataStore)
        {
            return PlayerMainPageViewModel.LoadViewModel(navigatorStore, subscriptionDataStore, playerStore, paymentDataStore, sportDataStore,licenseDataStore);
        }

        private SubscriptionDetailsViewModel LoadSubscriptionViewModel(NavigationStore navigatorStore, SportDataStore sportDataStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, PaymentDataStore paymentDataStore,PlayerMainPageViewModel playerMainPageViewModel)
        {
            return SubscriptionDetailsViewModel.LoadViewModel(sportDataStore, navigatorStore, subscriptionDataStore, playersDataStore, paymentDataStore,playerMainPageViewModel);
        }
        private AddPaymentViewModel LoadAddPaymentViewModel(PaymentDataStore paymentDataStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, NavigationStore navigatorStore, PaymentListViewModel paymentListViewModel)
        {
            return AddPaymentViewModel.LoadViewModel(paymentDataStore, subscriptionDataStore, playersDataStore, navigatorStore, paymentListViewModel);
        }
        private MetricReportViewModel LoadMetricsViewModel(MetricDataStore metricDataStore, PlayersDataStore playerDataStore, NavigationStore navigationStore)
        {
            return MetricReportViewModel.LoadViewModel(metricDataStore, playerDataStore, navigationStore);
        }
        private RoutinePlayerViewModels LoadRoutineViewModel(RoutineDataStore routineDataStore, PlayersDataStore playerDataStore, NavigationStore navigationStore,LicenseDataStore licenseDataStore )
        {
            return RoutinePlayerViewModels.LoadViewModel(routineDataStore, playerDataStore, navigationStore,licenseDataStore);
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
