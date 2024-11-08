using Unicepse.Core.Models.DailyActivity;
using Unicepse.Commands;
using Unicepse.Commands.PlayerAttendenceCommands;
using Unicepse.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.ViewModels;
using Unicepse.Commands.Employee;
using Unicepse.navigation.Stores;
using Unicepse.ViewModels.Employee.TrainersViewModels;
using Unicepse.Stores;
using Unicepse.ViewModels.PlayersAttendenceViewModels;
using Unicepse.Commands.Player;
using Unicepse.navigation;

namespace Unicepse.utlis.common
{
    public class HomeViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<PlayerAttendenceListItemViewModel> _playerAttendenceListItemViewModels;


        private readonly ObservableCollection<TrainerListItemViewModel> _trainerListItemViewModels;

        private readonly PlayersDataStore _playerStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly EmployeeStore _employeeStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly NavigationStore _navigationStore;
        private readonly PlayersDataStore? _playersDataStore;
        private readonly SportDataStore? _sportDataStore;
        private readonly PaymentDataStore? _paymentDataStore;
        private readonly MetricDataStore? _metricDataStore;
        private readonly RoutineDataStore? _routineDataStore;
        private readonly LicenseDataStore? _licenseDataStore;
        private readonly NavigationService<PlayerListViewModel>? _navigationService;


        public IEnumerable<PlayerAttendenceListItemViewModel> PlayerAttendence => _playerAttendenceListItemViewModels;

        public IEnumerable<TrainerListItemViewModel> TrainersList => _trainerListItemViewModels;
        public ICommand? OpenSearchListCommand { get; }
        public ICommand? LogPlayerCommand { get; }
        public ICommand LoadDailyReport { get; }
        public ICommand LoadTrainersCommand { get; }
        public ICommand OpenScanCommand { get; }
        public SearchBoxViewModel SearchBox { get; set; }
        public HomeViewModel(PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore, EmployeeStore employeeStore, NavigationStore navigationStore, SubscriptionDataStore subscriptionDataStore, SportDataStore? sportDataStore, PaymentDataStore? paymentDataStore, MetricDataStore? metricDataStore, RoutineDataStore? routineDataStore, LicenseDataStore? licenseDataStore, NavigationService<PlayerListViewModel>? navigationService)
        {
            _playerStore = playersDataStore;
            _employeeStore = employeeStore;
            _navigationStore = navigationStore;
            _playersAttendenceStore = playersAttendenceStore;
            _subscriptionDataStore = subscriptionDataStore;


            _playersAttendenceStore.Loaded += _playersAttendenceStore_Loaded;
            _playersAttendenceStore.LoggedIn += _playersAttendenceStore_LoggedIn;
            _playersAttendenceStore.LoggedOut += _playersAttendenceStore_LoggedOut;
            //OpenSearchListCommand = new OpenSearchCommand(new SearchPlayerWindowViewModel(_playerStore,_playersAttendenceStore, new NavigationStore()));
            OpenSearchListCommand = new NavaigateCommand<LogPlayerAttendenceViewModel>(new NavigationService<LogPlayerAttendenceViewModel>(_navigationStore, () => CreatePlayerSearchViewModel(_playerStore, _playersAttendenceStore, _navigationStore, this, _subscriptionDataStore)));
            OpenScanCommand = new LoginPlayerScanCommand(new ReadPlayerQrCodeViewModel(), _playersAttendenceStore, _playerStore);
            _playerAttendenceListItemViewModels = new ObservableCollection<PlayerAttendenceListItemViewModel>();
            _trainerListItemViewModels = new ObservableCollection<TrainerListItemViewModel>();
            LoadDailyReport = new GetLoggedPlayerCommand(_playersAttendenceStore, this);
            SearchBox = new SearchBoxViewModel();
            LoadTrainersCommand = new LoadTrainersCommand(_employeeStore, this);
            SelectedDate = DateTime.Now;

            _employeeStore.Loaded += _employeeStore_Loaded;
            _sportDataStore = sportDataStore;
            _paymentDataStore = paymentDataStore;
            _metricDataStore = metricDataStore;
            _routineDataStore = routineDataStore;
            _licenseDataStore = licenseDataStore;
            _navigationService = navigationService;
        }
        public override void Dispose()
        {

            _playersAttendenceStore.Loaded -= _playersAttendenceStore_Loaded;
            _playersAttendenceStore.LoggedIn -= _playersAttendenceStore_LoggedIn;
            _playersAttendenceStore.LoggedOut -= _playersAttendenceStore_LoggedOut;
            base.Dispose();
        }
        private void _employeeStore_Loaded()
        {
            _trainerListItemViewModels.Clear();
            foreach (var item in _employeeStore.Employees)
            {
                TrainerListItemViewModel trainerListItemViewModel = new(item);
                _trainerListItemViewModels.Add(trainerListItemViewModel);
            }
        }

        public PlayerAttendenceListItemViewModel? SelectedDailyPlayerReport
        {
            get
            {
                return PlayerAttendence
                    .FirstOrDefault(y => y?.dailyPlayerReport == _playersAttendenceStore.SelectedDailyPlayerReport);
            }
            set
            {
                _playersAttendenceStore.SelectedDailyPlayerReport = value?.dailyPlayerReport;
                OnPropertyChanged(nameof(SelectedDailyPlayerReport));
            }
        }
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                LoadDailyReport.Execute(null);
            }
        }
        private void _playersAttendenceStore_LoggedOut(DailyPlayerReport obj)
        {
            PlayerAttendenceListItemViewModel? itemViewModel = _playerAttendenceListItemViewModels.FirstOrDefault(y => y.dailyPlayerReport?.Id == obj.Id);

            if (itemViewModel != null)
            {
                itemViewModel.Update(obj);
            }
            loadData();
        }
        private void loadData()
        {
            _playerAttendenceListItemViewModels.Clear();
            foreach (var i in _playersAttendenceStore.PlayersAttendence.OrderByDescending(x => x.IsLogged).ThenByDescending(x => x.loginTime))
            {
                AddDailyPlayerLog(i);
            }
        }
        private void _playersAttendenceStore_LoggedIn(DailyPlayerReport dailyPlayerReport)
        {
            //AddDailyPlayerLog(dailyPlayerReport);
            loadData();
        }

        private void _playersAttendenceStore_Loaded()
        {
            loadData();
        }
        private void AddDailyPlayerLog(DailyPlayerReport dailyPlayerReport)
        {
            PlayerAttendenceListItemViewModel itemViewModel =
             new PlayerAttendenceListItemViewModel(dailyPlayerReport, _playersAttendenceStore);
            _playerAttendenceListItemViewModels.Add(itemViewModel);
            itemViewModel.IdSort = _playerAttendenceListItemViewModels.Count();
        }
        private static LogPlayerAttendenceViewModel CreatePlayerSearchViewModel(PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore, NavigationStore navigationStore, HomeViewModel homeViewModel, SubscriptionDataStore subscriptionDataStore)
        {

            return LogPlayerAttendenceViewModel.LoadViewModel(playersDataStore, playersAttendenceStore, navigationStore, homeViewModel, subscriptionDataStore);
        }

        public static HomeViewModel LoadViewModel(PlayersDataStore playersStore, PlayersAttendenceStore playersAttendenceStore, EmployeeStore employeeStore, NavigationStore navigationStore, SubscriptionDataStore subscriptionDataStore, SportDataStore? sportDataStore, PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, LicenseDataStore licenseDataStore, NavigationService<PlayerListViewModel> navigationService)
        {
            HomeViewModel viewModel = new(playersStore, playersAttendenceStore, employeeStore, navigationStore, subscriptionDataStore, sportDataStore, paymentDataStore, metricDataStore, routineDataStore, licenseDataStore, navigationService);

            viewModel.LoadDailyReport.Execute(null);
            viewModel.LoadTrainersCommand.Execute(null);
            return viewModel;
        }


    }
}
