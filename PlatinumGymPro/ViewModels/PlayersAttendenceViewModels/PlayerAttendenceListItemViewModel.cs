using PlatinumGym.Core.Models.DailyActivity;
using PlatinumGymPro.Commands.PlayerAttendenceCommands;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace PlatinumGymPro.ViewModels.PlayersAttendenceViewModels
{
    public class PlayerAttendenceListItemViewModel : ViewModelBase
    {
        public DailyPlayerReport dailyPlayerReport;
        private readonly PlayersAttendenceStore _playersAttendenceStore;

        public PlayerAttendenceListItemViewModel(DailyPlayerReport dailyPlayerReport, PlayersAttendenceStore playersAttendenceStore)
        {
            _playersAttendenceStore = playersAttendenceStore;
            this.dailyPlayerReport = dailyPlayerReport;
            LogoutCommand = new LogoutPlayerCommand(_playersAttendenceStore);
        }
        public ICommand LogoutCommand { get; }
        public string? Date => dailyPlayerReport.Date.ToShortDateString();
        public string? loginTime => dailyPlayerReport.loginTime.ToShortTimeString();
        public string? logoutTime => dailyPlayerReport.logoutTime.ToShortTimeString();
        public string? PlayerName=> dailyPlayerReport.Player!.FullName;
        public string? SubscribeEndDate => dailyPlayerReport.Player!.SubscribeEndDate.ToShortDateString();
        public bool IsLogged=> dailyPlayerReport.IsLogged;
        public Brush IsLoggedBrush => dailyPlayerReport.IsLogged ? Brushes.Green : Brushes.Red;
        public Brush IsSubscribed => dailyPlayerReport.Player!.IsSubscribed ? Brushes.Green : Brushes.Red;
        public int Id => dailyPlayerReport.Player!.Id;
    }
}
