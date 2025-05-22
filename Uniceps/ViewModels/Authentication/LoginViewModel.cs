using Uniceps.Commands;
using Uniceps.Commands.AuthCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.utlis.common;
using Uniceps.Stores;
using Uniceps.navigation.Stores;

namespace Uniceps.ViewModels.Authentication
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly AuthViewModel _authViewModel;
        private readonly NavigationStore _navigationStore;
        private readonly AuthenticationStore _authenticationStore;
        public LoginViewModel(AuthViewModel authViewModel, NavigationStore navigationStore, AuthenticationStore authenticationStore)
        {
            _authViewModel = authViewModel;
            _navigationStore = navigationStore;
            _authenticationStore = authenticationStore;
            RegisterCommand = new NavaigateCommand<RegisterViewModel>(new NavigationService<RegisterViewModel>(navigationStore, () => new RegisterViewModel(_authViewModel, navigationStore, _authenticationStore)));
            LoginCommand = new LoginCommand(_authViewModel, _authenticationStore, this);

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
                OnPropertyChanged(nameof(Password));
            }
        }
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
    }
}
