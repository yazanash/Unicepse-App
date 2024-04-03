using PlatinumGym.Core.Models.Authentication;
using PlatinumGymPro.Commands;
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
    public class UserListItemViewModel
    {
        public User user;
        private readonly NavigationStore _navigationStore;
        private readonly UsersDataStore _usersDataStore;
        private readonly UsersListViewModel _usersListViewModel;
        private readonly EmployeeStore _employeeStore;
        public int Id { get; }
        public string? username => user.UserName;
        public string? EmployeeName => user.Employee==null?"اسم الموظف": user.Employee!.FullName;
        public string? Role => "سكرتارية";
        public string? Postion => user.Employee == null ? "اسم الوظيفة" : user.Employee!.Position;
        public UserListItemViewModel(User user, NavigationStore navigationStore, UsersDataStore usersDataStore, UsersListViewModel usersListViewModel, EmployeeStore employeeStore)
        {
            this.user = user;
            _navigationStore = navigationStore;
            _usersDataStore = usersDataStore;
           _usersListViewModel = usersListViewModel;
           _employeeStore = employeeStore;
            EditUserCommand = new NavaigateCommand<EditUserViewModel>(new NavigationService<EditUserViewModel>(_navigationStore, () => CreateEditPlayerViewModel(_usersDataStore, _navigationStore, _usersListViewModel, _employeeStore)));
           
        }

        private EditUserViewModel CreateEditPlayerViewModel(UsersDataStore usersDataStore, NavigationStore navigatorStore, UsersListViewModel usersListViewModel, EmployeeStore employeeStore)
        {
            return EditUserViewModel.loadViewModel(usersDataStore,  navigatorStore,  usersListViewModel,  employeeStore);
        }

        public void update(User user)
        {
            this.user = user;
        }
        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
    }
}
