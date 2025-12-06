using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.ViewModels.PlayersAttendenceViewModels;
using Uniceps.Stores;
using Uniceps.Views.PlayersAttendenceViews;

namespace Uniceps.Commands.PlayerAttendenceCommands
{
    public class OpenAddKeyDialog : CommandBase
    {
        private readonly KeyDialogViewModel _viewModelBase;

        public OpenAddKeyDialog(KeyDialogViewModel viewModelBase)
        {
            _viewModelBase = viewModelBase;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                AttendanceKeyViewWindow cameraReader = new()
                {
                    DataContext = _viewModelBase
                };
                cameraReader.ShowDialog();
            }
            catch
            {
                //MessageBox.Show(ex.Message);
            }
        }

    }
}
