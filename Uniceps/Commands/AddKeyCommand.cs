using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Stores;
using Uniceps.ViewModels.PlayersAttendenceViewModels;
using Uniceps.Views.PlayersAttendenceViews;

namespace Uniceps.Commands
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
