using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.Authentication
{
    public class UsersViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly UsersDataStore _usersDataStore;
        private readonly EmployeeStore _employeeStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public UsersViewModel(NavigationStore navigatorStore, UsersDataStore usersDataStore, EmployeeStore employeeStore)
        {
            _navigatorStore = navigatorStore;
            _usersDataStore = usersDataStore;
            _employeeStore = employeeStore;

            navigatorStore.CurrentViewModel = CreateUserListViewModel(_navigatorStore, _usersDataStore, _employeeStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
        }
        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private UsersListViewModel CreateUserListViewModel(NavigationStore navigatorStore, UsersDataStore usersDataStore, EmployeeStore employeeStore)
        {
            return UsersListViewModel.LoadViewModel(navigatorStore, usersDataStore, employeeStore);
        }
    }
}
