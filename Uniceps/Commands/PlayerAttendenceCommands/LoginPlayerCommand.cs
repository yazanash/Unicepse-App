using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Uniceps.Core.Models.Player;
using Uniceps.Commands;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.navigation.Stores;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.Core.Models.DailyActivity;

namespace Uniceps.Commands.PlayerAttendenceCommands
{
    public class LoginPlayerCommand : AsyncCommandBase
    {
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly PlayersDataStore _playersDataStore;
        private NavigationService<HomeViewModel> _navigationService;
        PlayerListItemViewModel _playerListItemViewModel;
        private readonly PlayerProfileViewModel _playerProfileViewModel;
        private readonly NavigationStore _navigationStore;
        public LoginPlayerCommand(PlayersAttendenceStore playersAttendenceStore, PlayersDataStore playersDataStore, NavigationService<HomeViewModel> navigationService, PlayerListItemViewModel playerListItemViewModel, PlayerProfileViewModel playerProfileViewModel, NavigationStore navigationStore)
        {
            _playersAttendenceStore = playersAttendenceStore;
            _playersDataStore = playersDataStore;
            _navigationService = navigationService;
            _playerListItemViewModel = playerListItemViewModel;
            _playerProfileViewModel = playerProfileViewModel;
            _navigationStore = navigationStore;
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
                        _playersDataStore.SelectedPlayer = _playersDataStore.SelectedPlayer;
                        _playerProfileViewModel.SubscriptionCommand!.Execute(null);
                        NavigationService<PlayerProfileViewModel> nav = new NavigationService<PlayerProfileViewModel>(_navigationStore, () => _playerProfileViewModel);
                        nav.Navigate();
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
