using PlatinumGymPro.Models;
using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using PlatinumGymPro.ViewModels;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands
{
    public class LoadPlayersCommand : AsyncCommandBase
    {
        private readonly PlayerStore _playerStore;
        private readonly PlayerListViewModel _playerListing;
       

        public LoadPlayersCommand(PlayerStore playerStore, PlayerListViewModel playerListing)
        {
            _playerStore = playerStore;
            _playerListing = playerListing;
           
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _playerListing.ErrorMessage = null;
            _playerListing.IsLoading = true;

            try
            {
              
                await _playerStore.Load();
            }
            catch (Exception)
            {
                _playerListing.ErrorMessage = "Failed to load YouTube viewers. Please restart the application.";
            }
            finally
            {
                _playerListing.IsLoading = false;
            }
        }
    }
}
