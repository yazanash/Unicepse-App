using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores.PlayerStores;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.PlayersCommands
{
    public class OpenEditPlayerCommand : CommandBase
    {
        private readonly NavigationService<EditPlayerViewModel> navigationService;
        private readonly PlayerStore _playerStore;
        private PlayerListItemViewModel _playerListingItemViewModel;

        public OpenEditPlayerCommand(NavigationService<EditPlayerViewModel> navigationService, PlayerStore playerStore, PlayerListItemViewModel playerListingItemViewModel)
        {
            this.navigationService = navigationService;
            _playerStore = playerStore;
            _playerListingItemViewModel = playerListingItemViewModel;
        }

        public override void Execute(object? parameter)
        {
            Player player =_playerListingItemViewModel.Player;

        }
    }
}
