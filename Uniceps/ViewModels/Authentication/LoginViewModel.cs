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
using Uniceps.Stores;
using Uniceps.navigation.Stores;
using System.Collections.ObjectModel;

namespace Uniceps.ViewModels.Authentication
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly AuthViewModel _authViewModel;
        private readonly NavigationStore _navigationStore;
        private readonly AuthenticationStore _authenticationStore;
        private readonly UsersDataStore _usersDataStore;
        public ObservableCollection<string> _usersList = new ObservableCollection<string>();
        public IEnumerable<string> Users => _usersList;
        public LoginViewModel(AuthViewModel authViewModel, NavigationStore navigationStore, AuthenticationStore authenticationStore, UsersDataStore usersDataStore)
        {
            _authViewModel = authViewModel;
            _navigationStore = navigationStore;
            _authenticationStore = authenticationStore;
            RegisterCommand = new NavaigateCommand<RegisterViewModel>(new NavigationService<RegisterViewModel>(navigationStore, () => new RegisterViewModel(_authViewModel, navigationStore, _authenticationStore)));
            LoginCommand = new LoginCommand(_authViewModel, _authenticationStore, this);
            _usersDataStore = usersDataStore;
            _usersDataStore.Loaded += _usersDataStore_Loaded;
        }

        private void _usersDataStore_Loaded()
        {
            _usersList.Clear();
            foreach(string? username in _usersDataStore.Users.Select(x => x.UserName))
            {
                if (username != null)
                {
                    _usersList.Add(username);
                }
            }
        }

        public ICommand LoadUsersList => new AsyncRelayCommand(ExecuteLoadUsersCommand);

        private async Task ExecuteLoadUsersCommand()
        {
            await _usersDataStore.GetAll();
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
        public static LoginViewModel LoadViewModel(AuthViewModel authViewModel,NavigationStore navigationStore,AuthenticationStore authenticationStore,UsersDataStore usersDataStore)
        {
            LoginViewModel loginViewModel = new LoginViewModel(authViewModel,navigationStore,authenticationStore,usersDataStore);
            loginViewModel.LoadUsersList.Execute(null);
            return loginViewModel;
        }
    }
}
