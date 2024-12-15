using Unicepse.Core.Models.DailyActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.Commands;
using System.Windows.Navigation;
using Unicepse.utlis.common;
using Unicepse.navigation;
using Unicepse.ViewModels.PlayersViewModels;
using Unicepse.navigation.Stores;

namespace Unicepse.Commands.PlayerAttendenceCommands
{
    public class LoginPlayerCommand : AsyncCommandBase
    {
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly PlayersDataStore  _playersDataStore;
        private NavigationService<HomeViewModel> _navigationService;
        PlayerListItemViewModel _playerListItemViewModel;
        public LoginPlayerCommand(PlayersAttendenceStore playersAttendenceStore, PlayersDataStore playersDataStore, NavigationService<HomeViewModel> navigationService, PlayerListItemViewModel playerListItemViewModel)
        {
            _playersAttendenceStore = playersAttendenceStore;
            _playersDataStore = playersDataStore;
            _navigationService = navigationService;
            _playerListItemViewModel = playerListItemViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                if (_playersDataStore.SelectedPlayer!.SubscribeEndDate > DateTime.Now)
                {
                    DailyPlayerReport dailyPlayerReport = new DailyPlayerReport()
                    {
                        loginTime = DateTime.Now,
                        logoutTime = DateTime.Now,
                        Date = DateTime.Now,
                        IsLogged = true,
                        Player = _playersDataStore.SelectedPlayer,

                    };
                    await _playersAttendenceStore.LogInPlayer(dailyPlayerReport);
                    _navigationService.ReNavigate();
                }
                else
                {
                    if (MessageBox.Show("هذا اللاعب منتهي الاشتراك هل تريد اضافة اشتراك له ؟", "تنبيه", MessageBoxButton.YesNo,
                                   MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        _playerListItemViewModel.OpenProfileCommand.Execute(null);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
