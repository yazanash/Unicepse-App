using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.ViewModels.PlayersAttendenceViewModels;
using Unicepse.Views.PlayersAttendenceViews;

namespace Unicepse.Commands.PlayerAttendenceCommands
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
                AttendanceKeyViewWindow cameraReader = new AttendanceKeyViewWindow();
                cameraReader.DataContext = _viewModelBase;
                cameraReader.ShowDialog();
            }
            catch
            {
                //MessageBox.Show(ex.Message);
            }
        }
      
    }
}
