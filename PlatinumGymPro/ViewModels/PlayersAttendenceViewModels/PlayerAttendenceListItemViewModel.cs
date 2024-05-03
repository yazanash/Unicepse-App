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
        public DateTime Date => dailyPlayerReport.Date;
        public DateTime loginTime=> dailyPlayerReport.loginTime;
        public DateTime logoutTime=> dailyPlayerReport.logoutTime;
        public string? PlayerName=> dailyPlayerReport.Player!.FullName;
        public string? SubscribeEndDate => dailyPlayerReport.Player!.FullName;
        public bool IsLogged=> dailyPlayerReport.IsLogged;
        public Brush IsSubscribed => dailyPlayerReport.Player!.IsSubscribed ? Brushes.Green : Brushes.Red;
        public int Id => dailyPlayerReport.Player!.Id;
    }
}
