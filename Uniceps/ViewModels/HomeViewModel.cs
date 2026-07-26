using Uniceps.Commands;
using Uniceps.Commands.PlayerAttendenceCommands;
using Uniceps.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.Employee;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.Stores.RoutineStores;
using Uniceps.Stores;
using Uniceps.navigation.Stores;
using Uniceps.ViewModels.Employee.TrainersViewModels;
using Uniceps.ViewModels.PlayersAttendenceViewModels;
using Uniceps.Core.Models.DailyActivity;
using System.Windows;
using Uniceps.Views.PlayerViews;

namespace Uniceps.ViewModels
{
    public class HomeViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<PlayerAttendenceListItemViewModel> _playerAttendenceListItemViewModels;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly AccountStore _accountStore;
        private readonly Func<int,PlayerProfileViewModel> _playerProfileFactory;
        public IEnumerable<PlayerAttendenceListItemViewModel> PlayerAttendence => _playerAttendenceListItemViewModels;
        public ICommand LoadDailyReport { get; }
        public SearchBoxViewModel SearchBox { get; set; }
        public HomeViewModel(PlayersAttendenceStore playersAttendenceStore, AccountStore accountStore, Func<int, PlayerProfileViewModel> playerProfileFactory)
        {
            _playersAttendenceStore = playersAttendenceStore;
            _playersAttendenceStore.Loaded += _playersAttendenceStore_Loaded;
            _playersAttendenceStore.LoggedIn += _playersAttendenceStore_LoggedIn;
            _playersAttendenceStore.LoggedOut += _playersAttendenceStore_LoggedOut;
            _playerAttendenceListItemViewModels = new ObservableCollection<PlayerAttendenceListItemViewModel>();
            LoadDailyReport = new GetLoggedPlayerCommand(_playersAttendenceStore, this);

            SearchBox = new SearchBoxViewModel();
            _accountStore = accountStore;
            if (_accountStore.CurrentAccount != null)
            {
                HasPermission = _accountStore.CurrentAccount.Role == Core.Common.Roles.Admin || _accountStore.CurrentAccount.Role == Core.Common.Roles.Supervisor;
            }
            else
            {
                HasPermission = true;
            }
            LoadDailyReport.Execute(null);
            _playerProfileFactory = playerProfileFactory;
        }
        private bool _hasPermission = false;
        public bool HasPermission
        {
            get => _hasPermission;
            set { _hasPermission = value; OnPropertyChanged(nameof(HasPermission)); }
        }
        public override void Dispose()
        {

            _playersAttendenceStore.Loaded -= _playersAttendenceStore_Loaded;
            _playersAttendenceStore.LoggedIn -= _playersAttendenceStore_LoggedIn;
            _playersAttendenceStore.LoggedOut -= _playersAttendenceStore_LoggedOut;
            base.Dispose();
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
        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
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
                if (!HasPermission&&!i.IsLogged)
                {
                    continue;
                }
                AddDailyPlayerLog(i);

            }
        }
        private void _playersAttendenceStore_LoggedIn(DailyPlayerReport dailyPlayerReport)
        {
            loadData();
        }

        private void _playersAttendenceStore_Loaded()
        {
            loadData();
        }
        private void AddDailyPlayerLog(DailyPlayerReport dailyPlayerReport)
        {
            if (dailyPlayerReport.Date.Day == SelectedDate.Day && dailyPlayerReport.Date.Month == SelectedDate.Month && dailyPlayerReport.Date.Year == SelectedDate.Year)
            {
                PlayerAttendenceListItemViewModel itemViewModel =
                 new PlayerAttendenceListItemViewModel(dailyPlayerReport, _playersAttendenceStore);
                _playerAttendenceListItemViewModels.Add(itemViewModel);
                itemViewModel.IdSort = _playerAttendenceListItemViewModels.Count();
            }
        }
        public ICommand OpenProfileCommand => new RelayCommand<PlayerAttendenceListItemViewModel>(ExecuteOpenPlayerProfile);

        public void ExecuteOpenPlayerProfile(PlayerAttendenceListItemViewModel playerAttendenceListItemViewModel)
        {
                PlayerProfileWindowView playerProfileWindowView = new PlayerProfileWindowView();
                playerProfileWindowView.DataContext = _playerProfileFactory(playerAttendenceListItemViewModel.Id);
                playerProfileWindowView.ShowDialog();
        }
      
    }
}
