using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.WPF.utlis.common;
using Unicepse.WPF.navigation;
using Unicepse.WPF.ViewModels.Authentication;

namespace Unicepse.WPF.Commands.AuthCommands
{
    public class AuthCommand : AsyncCommandBase
    {
        private readonly NavigationService<MainViewModel> navigationService;
        private readonly AuthViewModel authViewModel;

        public AuthCommand(NavigationService<MainViewModel> navigationService, AuthViewModel auth)
        {
            authViewModel = auth;
            this.navigationService = navigationService;
        }
        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {

        }


    }
}
