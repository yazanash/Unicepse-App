using PlatinumGymPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands
{
    public class PrintCommand : CommandBase
    {
        private readonly ViewModelBase _viewModelBase;

        public PrintCommand(ViewModelBase viewModelBase)
        {
            _viewModelBase = viewModelBase;
        }

        public override void Execute(object? parameter)
        {
            PrintWindowDialog printWindowDialog = new PrintWindowDialog();
            printWindowDialog.DataContext = _viewModelBase;
            printWindowDialog.ShowDialog();
        }
    }
}
