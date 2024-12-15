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
        public LoadPlayersCommand(ListingViewModelBase playerListing, PlayersDataStore playerStore)
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
                await _playerStore.GetAll();
            }
            catch (Exception ex)
            {
                _playerListing.ErrorMessage = "خطأ في تحميل اللاعبين يرجى اعادة تشغيل البرنامج";
            }
            finally
            {
                _playerListing.IsLoading = false;
            }
        }
    }
}
