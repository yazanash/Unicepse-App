using PlatinumGym.Core.Models.DailyActivity;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.PlayerAttendenceCommands;
//using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using PlatinumGymPro.ViewModels.HomePageViewModels;
using PlatinumGymPro.ViewModels.PlayersAttendenceViewModels;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels
{
    public class HomeViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<PlayerAttendenceListItemViewModel> _playerAttendenceListItemViewModels;
        private readonly PlayersDataStore _playerStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        public IEnumerable<PlayerAttendenceListItemViewModel> PlayerAttendence => _playerAttendenceListItemViewModels;
        public ICommand? OpenSearchListCommand { get; }
        public ICommand LoadDailyReport { get; }
     
        public HomeViewModel(PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore)
        {
            _playerStore = playersDataStore;
            _playersAttendenceStore = playersAttendenceStore;
            _playersAttendenceStore.Loaded += _playersAttendenceStore_Loaded;
            _playersAttendenceStore.LoggedIn += _playersAttendenceStore_LoggedIn;
            _playersAttendenceStore.LoggedOut += _playersAttendenceStore_LoggedOut;
            OpenSearchListCommand = new OpenSearchCommand(new SearchPlayerWindowViewModel(CreatePlayerSearchViewModel(_playerStore,_playersAttendenceStore), new NavigationStore()));
            _playerAttendenceListItemViewModels = new ObservableCollection<PlayerAttendenceListItemViewModel>();
            LoadDailyReport = new GetLoggedPlayerCommand(_playersAttendenceStore);
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
             new PlayerAttendenceListItemViewModel(dailyPlayerReport,_playersAttendenceStore);
            _playerAttendenceListItemViewModels.Add(itemViewModel);
        }
        private static LogPlayerAttendenceViewModel CreatePlayerSearchViewModel( PlayersDataStore playersDataStore,PlayersAttendenceStore playersAttendenceStore)
        {
           
            return LogPlayerAttendenceViewModel.LoadViewModel( playersDataStore, playersAttendenceStore);
        }
        public static HomeViewModel LoadViewModel(PlayersDataStore playersStore, PlayersAttendenceStore playersAttendenceStore)
        {
            HomeViewModel viewModel = new(playersStore, playersAttendenceStore);

            viewModel.LoadDailyReport.Execute(null);

            return viewModel;
        }


    }
}
