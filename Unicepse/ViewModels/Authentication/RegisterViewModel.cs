using Unicepse.Commands;
using Unicepse.Commands.AuthCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.Authentication
{
    public class RegisterViewModel : ErrorNotifyViewModelBase
    {
        private readonly AuthViewModel _authViewModel;
        private readonly NavigationStore? _navigationStore;
        private readonly AuthenticationStore _authenticationStore;
        public RegisterViewModel(AuthViewModel authViewModel, NavigationStore navigationStore, AuthenticationStore authenticationStore)
        {
            _authViewModel = authViewModel;
            _navigationStore = navigationStore;
            _authenticationStore = authenticationStore;
            LoginCommand = new NavaigateCommand<LoginViewModel>(new NavigationService<LoginViewModel>(_navigationStore!, () => new LoginViewModel(_authViewModel, _navigationStore!, _authenticationStore)));
            RegisterCommand = new RegisterCommand(_authViewModel, authenticationStore, this, new NavigationService<LoginViewModel>(_navigationStore!, () => new LoginViewModel(_authViewModel, _navigationStore!, _authenticationStore)));

        }
        private string? _userName;
        public string? UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        private string? _password;
        public string? Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
              
            }
        }
        private string? _passwordConfirm;
        public string? PasswordConfirm
        {
            get
            {
                return _passwordConfirm;
            }
            set
            {
                _passwordConfirm = value;
              
            }
        }
        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
    }
}
