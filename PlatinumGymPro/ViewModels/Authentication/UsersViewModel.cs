using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.Authentication
{
    public class UsersViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly UsersDataStore _usersDataStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public UsersViewModel(NavigationStore navigatorStore, UsersDataStore usersDataStore)
        {
            _navigatorStore = navigatorStore;
            _usersDataStore = usersDataStore;
            navigatorStore.CurrentViewModel = CreateUserListViewModel(_navigatorStore,_usersDataStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;

        }
        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private UsersListViewModel CreateUserListViewModel(NavigationStore navigatorStore, UsersDataStore usersDataStore)
        {
            return UsersListViewModel.LoadViewModel(navigatorStore, usersDataStore);
        }
    }
}
