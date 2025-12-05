//using PlatinumGymPro.Models;
using Uniceps.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.navigation;
using Uniceps.Commands;
using Uniceps.ViewModels;

namespace Uniceps.Commands.Player
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
