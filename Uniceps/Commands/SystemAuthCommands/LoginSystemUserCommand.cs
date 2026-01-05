using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Services;
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
                if (ValidationService.IsValidEmail(_loginViewModel.Email ?? ""))
                {
                   bool requested = await _systemAuthStore.RequestOTP(_loginViewModel.Email!);
                    if (!requested)
                    {
                        MessageBox.Show("حدث خطأ غير متوقع يرجى التاكد من اتصال الانترنت او الاتصال بالدعم الفني ");
                    }
                }
                else
                    MessageBox.Show("البريد الالكتروني غير صحيح");

            }

            else
            {
                bool verified = await _systemAuthStore.VerifyOTP(_loginViewModel.Email!, _loginViewModel.OTP!);
                if (!verified)
                {
                    MessageBox.Show("حدث خطأ غير متوقع يرجى التاكد من اتصال الانترنت او الاتصال بالدعم الفني ");
                }
            }

            _loginViewModel.IsLoading = false;

        }
    }
}
