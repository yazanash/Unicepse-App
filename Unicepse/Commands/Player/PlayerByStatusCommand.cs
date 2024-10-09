using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.ViewModels.PlayersViewModels;

namespace Unicepse.Commands.Player
{
    public class PlayerByStatusCommand : AsyncCommandBase
    {
        //private readonly PlayerStore _playerStore;
        private readonly PlayerListViewModel _playerListing;
        private bool _status;
        public PlayerByStatusCommand(PlayerListViewModel playerListing, bool status)
        {
            //_playerStore = playerStore;
            _playerListing = playerListing;
            _status = status;
        }
        public async override Task ExecuteAsync(object? parameter)
        {
            _playerListing.ErrorMessage = null;
            _playerListing.IsLoading = true;

            try
            {
                await Task.Delay(5000);
                //await _playerStore.Load(_status);
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
