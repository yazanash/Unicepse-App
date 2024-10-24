﻿using Unicepse.Core.Models.DailyActivity;
using Unicepse.Commands.PlayerAttendenceCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Unicepse.Stores;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels.PlayersAttendenceViewModels
{
    public class PlayerAttendenceListItemViewModel : ViewModelBase
    {
        public DailyPlayerReport dailyPlayerReport { get; set; }
        private readonly PlayersAttendenceStore _playersAttendenceStore;

        public PlayerAttendenceListItemViewModel(DailyPlayerReport dailyPlayerReport, PlayersAttendenceStore playersAttendenceStore)
        {
            _playersAttendenceStore = playersAttendenceStore;
            this.dailyPlayerReport = dailyPlayerReport;
            this.IsTakenKey = dailyPlayerReport.IsTakenKey;
            this.Key = dailyPlayerReport.KeyNumber;
            this.IsLogged = this.dailyPlayerReport.IsLogged;
            this.logoutTime = dailyPlayerReport.IsLogged ? "لم يسجل خروج بعد" : dailyPlayerReport.logoutTime.ToShortTimeString();
            this.IsLoggedBrush = this.IsLogged ? Brushes.Green : Brushes.Red;
            LogoutCommand = new LogoutPlayerCommand(_playersAttendenceStore);
            AddKeyCommand = new OpenAddKeyDialog(new KeyDialogViewModel(this.dailyPlayerReport.Player!.FullName!, _playersAttendenceStore));
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
        public string? PlayerName => dailyPlayerReport.Player!.FullName;
        public string? SubscribeEndDate => dailyPlayerReport.Player!.SubscribeEndDate.ToShortDateString();
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
        public Brush IsSubscribed => dailyPlayerReport.Player!.IsSubscribed ? Brushes.Green : Brushes.Red;
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
            this.dailyPlayerReport = obj;
            this.IsTakenKey = this.dailyPlayerReport.IsTakenKey;
            this.Key = this.dailyPlayerReport.KeyNumber;
            this.IsLogged = this.dailyPlayerReport.IsLogged;
            this.logoutTime = dailyPlayerReport.IsLogged ? "لم يسجل خروج بعد" : dailyPlayerReport.logoutTime.ToShortTimeString();
            this.IsLoggedBrush = this.IsLogged ? Brushes.Green : Brushes.Red;
        }
    }
}
