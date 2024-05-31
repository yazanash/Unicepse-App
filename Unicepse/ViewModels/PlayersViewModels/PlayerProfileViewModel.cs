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
        public PlayerListItemViewModel? Player => _playersDataStore.SelectedPlayer;
        public ViewModelBase? CurrentPlayerViewModel => _navigatorStore.CurrentViewModel;

        public PlayerProfileViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionStore,
            PlayersDataStore playersDataStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore)
        {
            _navigatorStore = navigatorStore;
            _subscriptionStore = subscriptionStore;
            _playersDataStore = playersDataStore;
            _sportDataStore = sportDataStore;
            _paymentDataStore = paymentDataStore;
            _routineDataStore = routineDataStore;
            _metricDataStore = metricDataStore;
            _playersAttendenceStore = playersAttendenceStore;

            navigatorStore.CurrentViewModel = LoadPlayerMainPageViewModel(_navigatorStore, _playersDataStore, _subscriptionStore, _paymentDataStore, _sportDataStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
            _playersDataStore.PlayerChanged += _playersDataStore_PlayerChanged;
            PlayerHomeCommand = new NavaigateCommand<PlayerMainPageViewModel>(new NavigationService<PlayerMainPageViewModel>(_navigatorStore, () => LoadPlayerMainPageViewModel(_navigatorStore, _playersDataStore, _subscriptionStore, _paymentDataStore, _sportDataStore)));
            SubscriptionCommand = new NavaigateCommand<SubscriptionDetailsViewModel>(new NavigationService<SubscriptionDetailsViewModel>(_navigatorStore, () => LoadSubscriptionViewModel(_navigatorStore, _sportDataStore, _subscriptionStore, _playersDataStore, _paymentDataStore)));
            PaymentCommand = new NavaigateCommand<PaymentListViewModel>(new NavigationService<PaymentListViewModel>(_navigatorStore, () => LoadPaymentsViewModel(_paymentDataStore, _playersDataStore, _navigatorStore, _subscriptionStore)));
            MetricsCommand = new NavaigateCommand<MetricReportViewModel>(new NavigationService<MetricReportViewModel>(_navigatorStore, () => LoadMetricsViewModel(_metricDataStore, _playersDataStore, _navigatorStore)));
            TrainingProgramCommand = new NavaigateCommand<RoutinePlayerViewModels>(new NavigationService<RoutinePlayerViewModels>(_navigatorStore, () => LoadRoutineViewModel(_routineDataStore, _playersDataStore, _navigatorStore)));
            PlayerAttendenceCommand = new NavaigateCommand<PlayerAttendenceViewModel>(new NavigationService<PlayerAttendenceViewModel>(_navigatorStore, () => LoadPlayerAttendenceViewModel(_playersAttendenceStore, _playersDataStore)));
        }

        private PaymentListViewModel LoadPaymentsViewModel(PaymentDataStore paymentDataStore, PlayersDataStore playersDataStore, NavigationStore navigatorStore, SubscriptionDataStore subscriptionDataStore)
        {
            return PaymentListViewModel.LoadViewModel(paymentDataStore, playersDataStore, navigatorStore, subscriptionDataStore);
        }

        private void _playersDataStore_PlayerChanged(PlayerListItemViewModel? obj)
        {
            OnPropertyChanged(nameof(Player));
        }

        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentPlayerViewModel));
        }
        private PlayerMainPageViewModel LoadPlayerMainPageViewModel(NavigationStore navigatorStore, PlayersDataStore playerStore, SubscriptionDataStore subscriptionDataStore, PaymentDataStore paymentDataStore, SportDataStore sportDataStore)
        {
            return PlayerMainPageViewModel.LoadViewModel(navigatorStore, subscriptionDataStore, playerStore, paymentDataStore, sportDataStore);
        }

        private SubscriptionDetailsViewModel LoadSubscriptionViewModel(NavigationStore navigatorStore, SportDataStore sportDataStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, PaymentDataStore paymentDataStore)
        {
            return SubscriptionDetailsViewModel.LoadViewModel(sportDataStore, navigatorStore, subscriptionDataStore, playersDataStore, paymentDataStore);
        }
        private AddPaymentViewModel LoadAddPaymentViewModel(PaymentDataStore paymentDataStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, NavigationStore navigatorStore, PaymentListViewModel paymentListViewModel)
        {
            return AddPaymentViewModel.LoadViewModel(paymentDataStore, subscriptionDataStore, playersDataStore, navigatorStore, paymentListViewModel);
        }
        private MetricReportViewModel LoadMetricsViewModel(MetricDataStore metricDataStore, PlayersDataStore playerDataStore, NavigationStore navigationStore)
        {
            return MetricReportViewModel.LoadViewModel(metricDataStore, playerDataStore, navigationStore);
        }
        private RoutinePlayerViewModels LoadRoutineViewModel(RoutineDataStore routineDataStore, PlayersDataStore playerDataStore, NavigationStore navigationStore)
        {
            return RoutinePlayerViewModels.LoadViewModel(routineDataStore, playerDataStore, navigationStore);
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
