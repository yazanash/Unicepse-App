using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.ViewModels;

namespace Unicepse.Commands.Player
{
    public class LoadArchivedPlayersCommand : AsyncCommandBase
    {
        private readonly PlayersDataStore _playerStore;
        private readonly ListingViewModelBase _playerListing;


        public LoadArchivedPlayersCommand(ListingViewModelBase playerListing, PlayersDataStore playerStore)
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

                await _playerStore.GetArchivedPlayers();
            }
            catch (Exception)
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
