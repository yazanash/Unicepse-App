using Uniceps.Commands.PlayerAttendenceCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.Core.Models.Player;
using Uniceps.Stores.RoutineStores;
using Uniceps.Stores;
using Uniceps.navigation.Stores;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.Core.Models.DailyActivity;

namespace Uniceps.ViewModels.PlayersAttendenceViewModels
{
    public class PlayerAttendenceListItemViewModel : ViewModelBase
    {
        public DailyPlayerReport dailyPlayerReport { get; set; }
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        public ICommand? OpenProfileCommand { get; }


        public PlayerAttendenceListItemViewModel(DailyPlayerReport dailyPlayerReport, PlayersAttendenceStore playersAttendenceStore)
        {
            _playersAttendenceStore = playersAttendenceStore;
            this.dailyPlayerReport = dailyPlayerReport;
            IsTakenKey = dailyPlayerReport.IsTakenKey;
            Key = dailyPlayerReport.KeyNumber;
            IsLogged = this.dailyPlayerReport.IsLogged;
            logoutTime = dailyPlayerReport.IsLogged ? "لم يسجل خروج بعد" : dailyPlayerReport.logoutTime.ToShortTimeString();
            IsLoggedBrush = IsLogged ? Brushes.Green : Brushes.Red;
            LogoutCommand = new LogoutPlayerCommand(_playersAttendenceStore);
            AddKeyCommand = new OpenAddKeyDialog(new KeyDialogViewModel(this.dailyPlayerReport.PlayerName!, _playersAttendenceStore));
        }

        private int _idSort;
        public int IdSort
        {
            get { return _idSort; }
            set { _idSort = value; OnPropertyChanged(nameof(IdSort)); }
        }

        public ICommand LogoutCommand { get; }
        public ICommand AddKeyCommand { get; }

        public string? Date => dailyPlayerReport.Date.ToShortDateString();
        public string? loginTime => dailyPlayerReport.loginTime.ToShortTimeString();

        private string? _logoutTime;
        public string? logoutTime
        {
            get { return _logoutTime; }
            set { _logoutTime = value; OnPropertyChanged(nameof(logoutTime)); }
        }
        public string? PlayerName => dailyPlayerReport.PlayerName;
        public string? SportName => dailyPlayerReport.SportName;
        public string? Code => dailyPlayerReport.Code;
        private bool _isLogged;
        public bool IsLogged
        {
            get { return _isLogged; }
            set
            {
                _isLogged = value;
                OnPropertyChanged(nameof(IsLogged));
            }
        }
        private Brush? _isLoggedBrush;
        public Brush? IsLoggedBrush
        {
            get { return _isLoggedBrush; }
            set
            {
                _isLoggedBrush = value;
                OnPropertyChanged(nameof(IsLoggedBrush));
            }
        }
        public int Id => dailyPlayerReport.Id;

        private bool _isTakenKey;
        public bool IsTakenKey
        {
            get { return _isTakenKey; }
            set
            {
                _isTakenKey = value;
                OnPropertyChanged(nameof(IsTakenKey));
            }
        }

        private int _key;
        public int Key
        {
            get { return _key; }
            set
            {
                _key = value;
                OnPropertyChanged(nameof(Key));
            }
        }
        public void Update(DailyPlayerReport obj)
        {
            dailyPlayerReport = obj;
            IsTakenKey = dailyPlayerReport.IsTakenKey;
            Key = dailyPlayerReport.KeyNumber;
            IsLogged = dailyPlayerReport.IsLogged;
            logoutTime = dailyPlayerReport.IsLogged ? "لم يسجل خروج بعد" : dailyPlayerReport.logoutTime.ToShortTimeString();
            IsLoggedBrush = IsLogged ? Brushes.Green : Brushes.Red;
        }
    }
}
