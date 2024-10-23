using Unicepse.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.ViewModels.PlayersViewModels;
using Unicepse.Stores;
using Unicepse.navigation;

namespace Unicepse.Commands.Player
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
                    Core.Models.Player.Player player = _playerStore.SelectedPlayer!.Player;
                    player.SubscribeEndDate = DateTime.Now;
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
