//using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands
{
    public class NavaigateCommand<TViewModel>  : CommandBase where TViewModel:ViewModelBase
    {
        private readonly NavigationService<TViewModel> _navigationService;
        public NavaigateCommand(NavigationService<TViewModel> navigationService)
        {
            _navigationService=navigationService;
        }
        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}
