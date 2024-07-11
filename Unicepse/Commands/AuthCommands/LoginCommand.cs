using Unicepse.Entityframework.Services.AuthService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.ViewModels.Authentication;
using Unicepse.Commands;
using Unicepse.Core.Exceptions;

namespace Unicepse.Commands.AuthCommands
{
    public class LoginCommand : AsyncCommandBase
    {
        private readonly AuthenticationStore? _authenticationStore;
        private readonly AuthViewModel? _authViewModel;
        private readonly LoginViewModel _loginViewModel;
        public LoginCommand(AuthViewModel? authViewModel, AuthenticationStore authenticationStore, LoginViewModel loginViewModel)
        {
            _authenticationStore = authenticationStore;
            _authViewModel = authViewModel;
            _loginViewModel = loginViewModel;
        }
        public async override Task ExecuteAsync(object? parameter)
        {
            try
            {
                await _authenticationStore!.Login(_loginViewModel!.UserName!, _loginViewModel.Password!);
                if (_authenticationStore.CurrentAccount != null)
                {
                    _authViewModel!.OnLoginAction();
                }
            }
         catch (InvalidPasswordException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (UserNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
