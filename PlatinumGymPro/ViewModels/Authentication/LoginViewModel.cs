using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.AuthCommands;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.Authentication
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
            RegisterCommand = new NavaigateCommand<RegisterViewModel>(new NavigationService<RegisterViewModel>(navigationStore, () => new RegisterViewModel(_authViewModel,navigationStore,_authenticationStore)));
            LoginCommand = new LoginCommand(_authViewModel,_authenticationStore,this);
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
