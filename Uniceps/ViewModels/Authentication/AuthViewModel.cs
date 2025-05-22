using Uniceps.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.utlis.common;
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
        public AuthViewModel(AuthenticationStore authenticationStore)
        {
            _navigatorStore = new NavigationStore();
            _authenticationStore = authenticationStore;

            _navigatorStore.CurrentViewModel = new LoginViewModel(this, _navigatorStore, _authenticationStore);

            _navigatorStore.CurrentViewModelChanged += _navigatorStore_CurrentViewModelChanged; ;

        }
        public void openLog()
        {
            if (_authenticationStore.HasUser())
                _navigatorStore.CurrentViewModel = new LoginViewModel(this, _navigatorStore, _authenticationStore);
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
