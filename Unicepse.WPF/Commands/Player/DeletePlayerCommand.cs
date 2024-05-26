using Unicepse.Core.Exceptions;
using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.WPF.navigation;

namespace Unicepse.WPF.Commands.Player
{
    public class DeletePlayerCommand : AsyncCommandBase
    {
        private readonly NavigationService<PlayerListViewModel> navigationService;
        private readonly PlayersDataStore _playerStore;
        public DeletePlayerCommand(NavigationService<PlayerListViewModel> navigationService, PlayersDataStore playerStore)
        {
            this.navigationService = navigationService;
            _playerStore = playerStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (MessageBox.Show("سيتم حذف هذا اللاعب , هل انت متاكد", "تنبيه", MessageBoxButton.YesNo,
                                          MessageBoxImage.Warning) == MessageBoxResult.Yes)
                try
                {
                    Unicepse.Core.Models.Player.Player player = _playerStore.SelectedPlayer!.Player;
                    player.IsSubscribed = false;
                    await _playerStore.DeletePlayer(player);
                    navigationService.Navigate();
                }
                catch (NotExistException ex)
                {
                    MessageBox.Show(ex.Message);
                }

        }
    }
}
