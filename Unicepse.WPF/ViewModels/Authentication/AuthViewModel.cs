using Unicepse.WPF.Commands;
using Unicepse.WPF.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.WPF.navigation.Stores;
using Unicepse.WPF.utlis.common;

namespace Unicepse.WPF.ViewModels.Authentication
{
    public class AuthViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public event Action? LoginAction;
        private readonly AuthenticationStore _authenticationStore;
        public AuthViewModel(NavigationStore navigatorStore, AuthenticationStore authenticationStore)
        {
            _navigatorStore = navigatorStore;
            _authenticationStore = authenticationStore;
            _navigatorStore.CurrentViewModel = new LoginViewModel(this, navigatorStore, _authenticationStore);
            _navigatorStore.CurrentViewModelChanged += _navigatorStore_CurrentViewModelChanged; ;

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
