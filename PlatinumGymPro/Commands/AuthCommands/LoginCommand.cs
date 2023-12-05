using PlatinumGym.Entityframework.Services.AuthService;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels;
using PlatinumGymPro.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands.AuthCommands
{
    public class LoginCommand : AsyncCommandBase
    {
        private readonly AuthenticationStore? _authenticationStore;
        private readonly AuthViewModel? _authViewModel;
        private readonly LoginViewModel _loginViewModel;
        public LoginCommand(AuthViewModel? authViewModel, AuthenticationStore authenticationStore,LoginViewModel loginViewModel)
        {
            _authenticationStore = authenticationStore;
            _authViewModel = authViewModel;
            _loginViewModel = loginViewModel;
        }
        public async override Task ExecuteAsync(object? parameter)
        {
            
            await _authenticationStore!.Login(_loginViewModel!.UserName!, _loginViewModel.Password!);
            if (_authenticationStore.CurrentAccount != null)
            {
                _authViewModel!.OnLoginAction();
            }
        }
    }
}
