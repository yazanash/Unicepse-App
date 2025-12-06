using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.ViewModels.SystemAuthViewModels;
using Uniceps.Views;

namespace Uniceps.Commands.SystemAuthCommands
{
    public class OpenAuthCommand : CommandBase
    {
        private readonly SystemLoginViewModel _systemLoginViewModel;

        public OpenAuthCommand(SystemLoginViewModel systemLoginViewModel)
        {
            _systemLoginViewModel = systemLoginViewModel;
        }

        public override void Execute(object? parameter)
        {
          LicenseWindow licenseWindow = new LicenseWindow() { DataContext = _systemLoginViewModel };
            licenseWindow.ShowDialog();
        }
    }
}
