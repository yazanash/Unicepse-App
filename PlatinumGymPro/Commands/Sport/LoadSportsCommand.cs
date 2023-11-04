using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.HomePageViewModels;
using PlatinumGymPro.ViewModels.SportsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.SportsCommands
{
    public class LoadSportsCommand : AsyncCommandBase
    {
        private readonly SportStore _sportStore;
        private readonly SportListViewModel _sportListing;

        public LoadSportsCommand(SportStore sportStore, SportListViewModel sportListing)
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
                await _sportStore.Load();
            }
            catch (Exception)
            {
                _sportListing.ErrorMessage = "Failed to load YouTube viewers. Please restart the application.";
            }
            finally
            {
                _sportListing.IsLoading = false;
            }
        }
    }
}
