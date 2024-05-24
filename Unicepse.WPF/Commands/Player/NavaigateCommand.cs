//using PlatinumGymPro.Models;
using Unicepse.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.WPF.utlis.common;
using Unicepse.WPF.navigation;

namespace Unicepse.WPF.Commands.Player
{
    public class NavaigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase
    {
        private readonly NavigationService<TViewModel> _navigationService;
        public NavaigateCommand(NavigationService<TViewModel> navigationService)
        {
            _navigationService = navigationService;
        }
        public override void Execute(object? parameter)
        {
            _navigationService.ReNavigate();
        }
    }
}
