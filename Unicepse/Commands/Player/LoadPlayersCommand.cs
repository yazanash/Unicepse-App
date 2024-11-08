using Unicepse.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.ViewModels;
using Unicepse.Commands;
using Microsoft.Extensions.Logging;

namespace Unicepse.Commands.Player
{
    public class LoadPlayersCommand : AsyncCommandBase
    {
        private readonly PlayersDataStore _playerStore;
        private readonly ListingViewModelBase _playerListing;
        private readonly ILogger _logger;
        public LoadPlayersCommand(ListingViewModelBase playerListing, PlayersDataStore playerStore, ILogger logger)
        {
            _playerStore = playerStore;
            _playerListing = playerListing;
            _logger = logger;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _playerListing.ErrorMessage = null;
            _playerListing.IsLoading = true;
            _logger.LogInformation("Load Players Command");
            try
            {          
                await _playerStore.GetPlayers();
            }
            catch (Exception ex)
            {
                _logger.LogError("Load Players error {0}",ex.Message);
                _playerListing.ErrorMessage = "خطأ في تحميل اللاعبين يرجى اعادة تشغيل البرنامج";
            }
            finally
            {
                _playerListing.IsLoading = false;
            }
        }
    }
}
