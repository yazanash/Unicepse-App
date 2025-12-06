using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.Authentication;
using Uniceps.Core.Common;
using Uniceps.Entityframework.Services.AuthService;

namespace Uniceps.Commands.AuthCommands
{
    public class RegisterCommand : AsyncCommandBase
    {
        private readonly AuthViewModel _authViewModel;
        private readonly AuthenticationStore _authenticationStore;
        private readonly RegisterViewModel _registerViewModel;
        NavigationService<LoginViewModel> _navigationService;

        public RegisterCommand(AuthViewModel authViewModel, AuthenticationStore authenticationStore, RegisterViewModel registerViewModel, NavigationService<LoginViewModel> navigationService)
        {
            _authViewModel = authViewModel;
            _authenticationStore = authenticationStore;
            _registerViewModel = registerViewModel;
            _navigationService = navigationService;
            _registerViewModel.PropertyChanged += _registerViewModel_PropertyChanged;
        }
        private void _registerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_registerViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }


        public override bool CanExecute(object? parameter)
        {

            return _registerViewModel.CanSubmit && base.CanExecute(null);
        }
        public async override Task ExecuteAsync(object? parameter)
        {
            try
            {

                RegistrationResult result = await _authenticationStore.Register(_registerViewModel.UserName!,
               _registerViewModel.Password!, _registerViewModel.PasswordConfirm!, Roles.Admin);
                if (result == RegistrationResult.Success)
                {
                    _navigationService.Navigate();
                }
                else if (result == RegistrationResult.UsernameAlreadyExists)
                    MessageBox.Show("هذا الحساب موجود بالفعل");
                else if (result == RegistrationResult.PasswordsDoNotMatch)
                    MessageBox.Show("كلمة المرور غير متطابقة");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
