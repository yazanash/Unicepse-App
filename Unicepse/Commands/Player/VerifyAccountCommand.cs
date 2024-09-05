using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.ViewModels.PlayersViewModels;

namespace Unicepse.Commands.Player
{
    public class VerifyAccountCommand : AsyncCommandBase
    {
        private readonly ReadPlayerQrCodeViewModel _viewModelBase;
        private readonly PlayersDataStore _playersDataStore;

        public VerifyAccountCommand(ReadPlayerQrCodeViewModel viewModelBase, PlayersDataStore playersDataStore)
        {
            _viewModelBase = viewModelBase;
            _playersDataStore = playersDataStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            CameraReader cameraReader = new CameraReader();
            cameraReader.DataContext = _viewModelBase;
            cameraReader.ShowDialog();
            await _playersDataStore.HandShakePlayer(_playersDataStore.SelectedPlayer!.Player!, _viewModelBase.UID!);
        }
      
    }
}
