using PlatinumGymPro.State.Navigator;
using PlatinumGymPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands
{
    internal class PlayerNavigatorCommand<TView> : CommandBase where TView : ViewModelBase
    {
        public INavigator _navigator;

        public PlayerNavigatorCommand(INavigator navigator)
        {
            _navigator = navigator;

        }

        public override void Execute(object? parameter)
        {
            //_navigator.CurrentViewModel = TView;
        }
    }
}
