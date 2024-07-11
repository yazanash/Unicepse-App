using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;
using Unicepse;

namespace Unicepse.Commands
{
    public class PrintCommand : CommandBase
    {
        private readonly ViewModelBase _viewModelBase;
        string FileName = "file_name";
        public PrintCommand(ViewModelBase viewModelBase)
        {
            _viewModelBase = viewModelBase;
        }
        public PrintCommand(ViewModelBase viewModelBase, string FileName)
        {
            _viewModelBase = viewModelBase;
            this.FileName = FileName;
        }
        public override void Execute(object? parameter)
        {
            PrintWindowDialog printWindowDialog = new PrintWindowDialog(FileName);
            printWindowDialog.DataContext = _viewModelBase;
            printWindowDialog.ShowDialog();
        }
    }
}
