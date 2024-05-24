using Unicepse.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.WPF.utlis.common;

namespace Unicepse.WPF.Commands.Player
{
    public class LoadActivePlayersCommand : AsyncCommandBase
    {
        //private readonly PlayersDailyStore _playersDailyStore;
        private readonly HomeViewModel _homeViewModel;

        public LoadActivePlayersCommand(HomeViewModel homeViewModel)
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
                throw new NotImplementedException();
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
