using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;
using Unicepse;

namespace Unicepse.Commands
{
    public class OpenSearchCommand : CommandBase
    {
        private readonly ViewModelBase _viewModelBase;

        public OpenSearchCommand(ViewModelBase viewModelBase)
        {
            _viewModelBase = viewModelBase;
        }

        public override void Execute(object? parameter)
        {
            SearchWindow searchWindow = new SearchWindow();
            searchWindow.DataContext = _viewModelBase;
            searchWindow.ShowDialog();
        }
    }
}
