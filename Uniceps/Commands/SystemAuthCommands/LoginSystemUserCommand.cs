using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Stores.SystemAuthStores;
using Uniceps.ViewModels.SystemAuthViewModels;

namespace Uniceps.Commands.SystemAuthCommands
{
    public class LoginSystemUserCommand : AsyncCommandBase
    {
        private readonly ISystemAuthStore _systemAuthStore;
        private readonly SystemLoginViewModel _loginViewModel;

        public LoginSystemUserCommand(ISystemAuthStore systemAuthStore, SystemLoginViewModel loginViewModel)
        {
            _systemAuthStore = systemAuthStore;
            _loginViewModel = loginViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _loginViewModel.IsLoading = true;

            if (!_loginViewModel.IsOTPRequested)
            {
                await _systemAuthStore.RequestOTP(_loginViewModel.Email!);
                _loginViewModel.IsLoading = false;

            }

            else
            {
                await _systemAuthStore.VerifyOTP(_loginViewModel.Email!, _loginViewModel.OTP!);
                _loginViewModel.IsLoading = false;
            }

        }
    }
}
