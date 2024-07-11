﻿using Unicepse.Core.Models.DailyActivity;
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
        private readonly EmployeeStore _employeeStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly NavigationStore _navigationStore;
        public IEnumerable<PlayerAttendenceListItemViewModel> PlayerAttendence => _playerAttendenceListItemViewModels;

        public IEnumerable<TrainerListItemViewModel> TrainersList => _trainerListItemViewModels;
        public ICommand? OpenSearchListCommand { get; }
        public ICommand? LogPlayerCommand { get; }
        public ICommand LoadDailyReport { get; }
        public ICommand LoadTrainersCommand { get; }
        public SearchBoxViewModel SearchBox { get; set; }
        public HomeViewModel(PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore, EmployeeStore employeeStore, NavigationStore navigationStore)
        {
            _playerStore = playersDataStore;
            _employeeStore = employeeStore;
            _navigationStore = navigationStore;
            _playersAttendenceStore = playersAttendenceStore;
            _playersAttendenceStore.Loaded += _playersAttendenceStore_Loaded;
            _playersAttendenceStore.LoggedIn += _playersAttendenceStore_LoggedIn;
            _playersAttendenceStore.LoggedOut += _playersAttendenceStore_LoggedOut;
            //OpenSearchListCommand = new OpenSearchCommand(new SearchPlayerWindowViewModel(_playerStore,_playersAttendenceStore, new NavigationStore()));
            OpenSearchListCommand = new NavaigateCommand<LogPlayerAttendenceViewModel>(new NavigationService<LogPlayerAttendenceViewModel>(_navigationStore, () => CreatePlayerSearchViewModel(_playerStore, _playersAttendenceStore,_navigationStore,this)));

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
        private static LogPlayerAttendenceViewModel CreatePlayerSearchViewModel(PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore,NavigationStore navigationStore,HomeViewModel homeViewModel)
        {

            return LogPlayerAttendenceViewModel.LoadViewModel(playersDataStore, playersAttendenceStore,navigationStore,homeViewModel);
        }

        public static HomeViewModel LoadViewModel(PlayersDataStore playersStore, PlayersAttendenceStore playersAttendenceStore, EmployeeStore employeeStore,NavigationStore navigationStore)
        {
            HomeViewModel viewModel = new(playersStore, playersAttendenceStore, employeeStore, navigationStore);

            viewModel.LoadDailyReport.Execute(null);
            viewModel.LoadTrainersCommand.Execute(null);
            return viewModel;
        }


    }
}
