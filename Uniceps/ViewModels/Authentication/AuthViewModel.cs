using Uniceps.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Stores;
using Uniceps.navigation.Stores;

namespace Uniceps.ViewModels.Authentication
{
    public class AuthViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public event Action? LoginAction;
        private readonly AuthenticationStore _authenticationStore;
        private readonly UsersDataStore _usersDataStore;
        public AuthViewModel(AuthenticationStore authenticationStore, UsersDataStore usersDataStore)
        {
            _navigatorStore = new NavigationStore();
            _authenticationStore = authenticationStore;
            _usersDataStore = usersDataStore;

            _navigatorStore.CurrentViewModel =  LoginViewModel.LoadViewModel(this, _navigatorStore, _authenticationStore, _usersDataStore);

            _navigatorStore.CurrentViewModelChanged += _navigatorStore_CurrentViewModelChanged; ;
        }
        public async Task openLog()
        {
            if (await _authenticationStore.HasUser())
                _navigatorStore.CurrentViewModel = LoginViewModel.LoadViewModel(this, _navigatorStore, _authenticationStore, _usersDataStore);
            else
                _navigatorStore.CurrentViewModel = new RegisterViewModel(this, _navigatorStore, _authenticationStore);
        }
        private void _navigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public void OnLoginAction()
        {
            LoginAction?.Invoke();
        }
        //public ICommand AuthCommand { get; }
    }
}
