using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands.Player
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
            PlatinumGym.Core.Models.Player.Player player = _playerStore.SelectedPlayer!.Player;
            player.IsSubscribed = false;
            await _playerStore.UpdatePlayer(player);
            MessageBox.Show(player.FullName + "deleted successfully");
            navigationService.Navigate();
        }
    }
}
