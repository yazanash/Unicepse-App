using PlatinumGymPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands
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
