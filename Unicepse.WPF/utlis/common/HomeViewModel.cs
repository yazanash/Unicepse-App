using Unicepse.Core.Models.DailyActivity;
using Unicepse.WPF.Commands;
using Unicepse.WPF.Commands.PlayerAttendenceCommands;
using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.PlayersAttendenceViewModels;
using Unicepse.WPF.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.WPF.ViewModels.Employee.TrainersViewModels;
using Unicepse.WPF.ViewModels;
using Unicepse.WPF.Commands.Employee;
using Unicepse.WPF.navigation.Stores;

namespace Unicepse.WPF.utlis.common
{
    public class HomeViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<PlayerAttendenceListItemViewModel> _playerAttendenceListItemViewModels;


        private readonly ObservableCollection<TrainerListItemViewModel> _trainerListItemViewModels;

        private readonly PlayersDataStore _playerStore;
        private readonly EmployeeStore _employeeStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        public IEnumerable<PlayerAttendenceListItemViewModel> PlayerAttendence => _playerAttendenceListItemViewModels;

        public IEnumerable<TrainerListItemViewModel> TrainersList => _trainerListItemViewModels;
        public ICommand? OpenSearchListCommand { get; }
        public ICommand? LogPlayerCommand { get; }
        public ICommand LoadDailyReport { get; }
        public ICommand LoadTrainersCommand { get; }
        public SearchBoxViewModel SearchBox { get; set; }
        public HomeViewModel(PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore, EmployeeStore employeeStore)
        {
            _playerStore = playersDataStore;
            _employeeStore = employeeStore;

            _playersAttendenceStore = playersAttendenceStore;
            _playersAttendenceStore.Loaded += _playersAttendenceStore_Loaded;
            _playersAttendenceStore.LoggedIn += _playersAttendenceStore_LoggedIn;
            _playersAttendenceStore.LoggedOut += _playersAttendenceStore_LoggedOut;
            OpenSearchListCommand = new OpenSearchCommand(new SearchPlayerWindowViewModel(CreatePlayerSearchViewModel(_playerStore, _playersAttendenceStore), new NavigationStore()));
            _playerAttendenceListItemViewModels = new ObservableCollection<PlayerAttendenceListItemViewModel>();
            _trainerListItemViewModels = new ObservableCollection<TrainerListItemViewModel>();
            LoadDailyReport = new GetLoggedPlayerCommand(_playersAttendenceStore);
            SearchBox = new SearchBoxViewModel();
            LoadTrainersCommand = new LoadTrainersCommand(_employeeStore, this);
            _employeeStore.Loaded += _employeeStore_Loaded;
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
        private void _playersAttendenceStore_LoggedOut(int obj)
        {
            PlayerAttendenceListItemViewModel? itemViewModel = _playerAttendenceListItemViewModels.FirstOrDefault(y => y.dailyPlayerReport?.Id == obj);

            if (itemViewModel != null)
            {
                _playerAttendenceListItemViewModels.Remove(itemViewModel);
            }
        }

        private void _playersAttendenceStore_LoggedIn(DailyPlayerReport dailyPlayerReport)
        {
            AddDailyPlayerLog(dailyPlayerReport);
        }

        private void _playersAttendenceStore_Loaded()
        {
            _playerAttendenceListItemViewModels.Clear();

            foreach (DailyPlayerReport dailyPlayerReport in _playersAttendenceStore.PlayersAttendence)
            {
                AddDailyPlayerLog(dailyPlayerReport);
            }
        }
        private void AddDailyPlayerLog(DailyPlayerReport dailyPlayerReport)
        {
            PlayerAttendenceListItemViewModel itemViewModel =
             new PlayerAttendenceListItemViewModel(dailyPlayerReport, _playersAttendenceStore);
            _playerAttendenceListItemViewModels.Add(itemViewModel);
        }
        private static LogPlayerAttendenceViewModel CreatePlayerSearchViewModel(PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore)
        {

            return LogPlayerAttendenceViewModel.LoadViewModel(playersDataStore, playersAttendenceStore);
        }
        public static HomeViewModel LoadViewModel(PlayersDataStore playersStore, PlayersAttendenceStore playersAttendenceStore, EmployeeStore employeeStore)
        {
            HomeViewModel viewModel = new(playersStore, playersAttendenceStore, employeeStore);

            viewModel.LoadDailyReport.Execute(null);
            viewModel.LoadTrainersCommand.Execute(null);
            return viewModel;
        }


    }
}
