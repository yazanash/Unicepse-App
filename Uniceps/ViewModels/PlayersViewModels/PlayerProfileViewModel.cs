using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Core.Models.Player;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels.Metrics;
using Uniceps.ViewModels.PaymentsViewModels;
using Uniceps.ViewModels.PlayersAttendenceViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;

namespace Uniceps.ViewModels.PlayersViewModels
{
    public class PlayerProfileViewModel : ViewModelBase
    {
        private NavigationStore _navigatorStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly LicenseStore _licenseStore;
        private PlayerMainPageViewModel _playerMainPageViewModel;
        public PlayerListItemViewModel? Player { get; set; }
        public ViewModelBase? CurrentPlayerViewModel => _navigatorStore.CurrentViewModel;
        public int PlayerId { get; }

        private readonly Func<int, PlayerMainPageViewModel> _mainPageFactory;
       
        private readonly Func<int, PaymentListViewModel> _paymentFactory;
        private readonly Func<int, MetricReportViewModel> _metricsFactory;
        private readonly Func<int, PlayerAttendenceViewModel> _attendanceFactory;
        private readonly Func<PremiumViewModel> _premiumFactory;

        public PlayerProfileViewModel(int playerId,
            PlayersDataStore playersDataStore, LicenseStore licenseStore, Func<int, PlayerMainPageViewModel> mainPageFactory, Func<int, PaymentListViewModel> paymentFactory, Func<int, MetricReportViewModel> metricsFactory, Func<int, PlayerAttendenceViewModel> attendanceFactory, Func<PremiumViewModel> premiumFactory)
        {
            PlayerId = playerId;
            
            _navigatorStore = new NavigationStore();
            _playersDataStore = playersDataStore;
            _mainPageFactory = mainPageFactory;
            _paymentFactory = paymentFactory;
            _metricsFactory = metricsFactory;
            _attendanceFactory = attendanceFactory;
            _premiumFactory = premiumFactory;
            _playerMainPageViewModel = _mainPageFactory(PlayerId);
            _navigatorStore.CurrentViewModel = _playerMainPageViewModel;
            _navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
            _playersDataStore.Player_update += _playersDataStore_Player_update;
            PlayerHomeCommand = new NavaigateCommand<PlayerMainPageViewModel>(new NavigationService<PlayerMainPageViewModel>(_navigatorStore, () => _playerMainPageViewModel));
            PaymentCommand = new NavaigateCommand<PaymentListViewModel>(new NavigationService<PaymentListViewModel>(_navigatorStore,()=> _paymentFactory(PlayerId)));
            _licenseStore = licenseStore;
            GetPlayerByIdCommand.Execute(PlayerId);
            if (_licenseStore.Current.IsFullVersion)
            {
                MetricsCommand = new NavaigateCommand<MetricReportViewModel>(new NavigationService<MetricReportViewModel>(_navigatorStore,()=>  _metricsFactory(PlayerId)));
                PlayerAttendenceCommand = new NavaigateCommand<PlayerAttendenceViewModel>(new NavigationService<PlayerAttendenceViewModel>(_navigatorStore, () => _attendanceFactory(PlayerId)));

            }
            else
            {
                MetricsCommand = new NavaigateCommand<PremiumViewModel>(new NavigationService<PremiumViewModel>(_navigatorStore, _premiumFactory));
                PlayerAttendenceCommand = new NavaigateCommand<PremiumViewModel>(new NavigationService<PremiumViewModel>(_navigatorStore, _premiumFactory));

            }

          
        }
        public ICommand GetPlayerByIdCommand => new AsyncRelayCommand<int>(GetPlayerById);

        private async Task GetPlayerById(int id)
        {
            Player player = await _playersDataStore.GetPlayerById(id);
            Player = new PlayerListItemViewModel(player,_playersDataStore);
        }

        public bool IsPersonal {get;set;}
        public bool IsMetrics { get; set; }
        public bool IsPayments { get; set; }
        public bool IsLog { get; set; }
        private void _playersDataStore_Player_update(Player obj)
        {
            if (Player != null && Player.Player.Id == obj.Id)
                Player.Update(obj);
        }
        private void NavigatorStore_CurrentViewModelChanged()
        {
            IsPersonal = CurrentPlayerViewModel is PlayerMainPageViewModel;
            IsMetrics = CurrentPlayerViewModel is MetricReportViewModel|| CurrentPlayerViewModel is PremiumViewModel;
            IsPayments = CurrentPlayerViewModel is PaymentListViewModel;
            IsLog = CurrentPlayerViewModel is PlayerAttendenceViewModel;
            OnPropertyChanged(nameof(CurrentPlayerViewModel));
            OnPropertyChanged(nameof(IsPersonal));
            OnPropertyChanged(nameof(IsMetrics));
            OnPropertyChanged(nameof(IsPayments));
            OnPropertyChanged(nameof(IsLog));
        }
        
        public ICommand? PlayerHomeCommand { get; }
        public ICommand? PaymentCommand { get; }
        public ICommand? MetricsCommand { get; }
        public ICommand? TrainingProgramCommand { get; }
        public ICommand? PlayerAttendenceCommand { get; }
    }
}
