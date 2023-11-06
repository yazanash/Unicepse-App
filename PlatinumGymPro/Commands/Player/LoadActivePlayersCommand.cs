using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using PlatinumGymPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.HomeCommands
{
    public class LoadActivePlayersCommand : AsyncCommandBase
    {
        //private readonly PlayersDailyStore _playersDailyStore;
        private readonly HomeViewModel _homeViewModel;

        public LoadActivePlayersCommand( HomeViewModel homeViewModel)
        {
            //_playersDailyStore = playersDailyStore;
            _homeViewModel = homeViewModel;

        }
        public override async Task ExecuteAsync(object? parameter)
        {
            _homeViewModel.ErrorMessage = null;
            //_homeViewModel.IsLoading = true;

            try
            {

                //await _playersDailyStore.LoadActivePlayerReport();
            }
            catch (Exception)
            {
                _homeViewModel.ErrorMessage = "Failed to load Players. Please restart the application.";
            }
            finally
            {
                //_homeViewModel.IsLoading = false;
            }
        }
    }
}
