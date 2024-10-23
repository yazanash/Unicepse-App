using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.ViewModels.PlayersAttendenceViewModels;
using Unicepse.Views.PlayersAttendenceViews;

namespace Unicepse.Commands
{
    public class AddKeyCommand : AsyncCommandBase
    {
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly KeyDialogViewModel _keyDialogViewModel;
        public AddKeyCommand(PlayersAttendenceStore playersAttendenceStore, KeyDialogViewModel keyDialogViewModel)
        {
            _playersAttendenceStore = playersAttendenceStore;
            _keyDialogViewModel = keyDialogViewModel;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            try
            {
                if (_playersAttendenceStore.SelectedDailyPlayerReport != null)
                {
                    _playersAttendenceStore.SelectedDailyPlayerReport!.KeyNumber = _keyDialogViewModel.Key;
                    _playersAttendenceStore.SelectedDailyPlayerReport!.IsTakenKey = true;
                    await _playersAttendenceStore.AddKeyToPlayer(_playersAttendenceStore.SelectedDailyPlayerReport!);
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is AttendanceKeyViewWindow)
                        {
                            window.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
