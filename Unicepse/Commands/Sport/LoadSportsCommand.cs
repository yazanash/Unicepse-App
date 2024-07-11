using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.ViewModels.SportsViewModels;
using Unicepse.Stores;

namespace Unicepse.Commands.Sport
{
    public class LoadSportsCommand : AsyncCommandBase
    {
        private readonly SportDataStore _sportStore;
        private readonly SportListViewModel _sportListing;

        public LoadSportsCommand(SportListViewModel sportListing, SportDataStore sportStore)
        {
            _sportStore = sportStore;
            _sportListing = sportListing;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _sportListing.ErrorMessage = null;
            _sportListing.IsLoading = true;

            try
            {
                await _sportStore.GetAll();
            }
            catch (Exception)
            {
                _sportListing.ErrorMessage = "Failed to load Sports . Please restart the application.";
            }
            finally
            {
                _sportListing.IsLoading = false;
            }
        }
    }
}
