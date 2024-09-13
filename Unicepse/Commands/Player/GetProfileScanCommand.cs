using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.ViewModels.PlayersViewModels;

namespace Unicepse.Commands.Player
{
    public class GetProfileScanCommand : AsyncCommandBase
    {
        private readonly ReadPlayerQrCodeViewModel _viewModelBase;
        private readonly PlayersDataStore _playersDataStore;

        public GetProfileScanCommand(ReadPlayerQrCodeViewModel viewModelBase, PlayersDataStore playersDataStore)
        {
            _viewModelBase = viewModelBase;
            _playersDataStore = playersDataStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            try
            {
                CameraReader cameraReader = new CameraReader();
                cameraReader.DataContext = _viewModelBase;
                cameraReader.ShowDialog();
                if (!string.IsNullOrEmpty(_viewModelBase.UID))
                {
                    string? uid = _viewModelBase.UID;
                    await _playersDataStore.GetPlayerProfile(uid!);
                }
                _viewModelBase.UID = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
