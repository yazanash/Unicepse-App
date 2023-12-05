using PlatinumGymPro.Commands;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using PlatinumGymPro.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels
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
            _navigatorStore.CurrentViewModel = new LoginViewModel(this, navigatorStore,_authenticationStore);
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
