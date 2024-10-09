using Unicepse.Core.Models.Authentication;
using Unicepse.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.Stores;
using Unicepse.navigation.Stores;
using Unicepse.Commands.AuthCommands;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels.Authentication
{
    public class UserListItemViewModel : ViewModelBase
    {
        public User user;
        private readonly NavigationStore _navigationStore;
        private readonly UsersDataStore _usersDataStore;
        private readonly UsersListViewModel _usersListViewModel;
        private readonly EmployeeStore _employeeStore;
        private readonly AuthenticationStore _authenticationStore;
        private int _order;
        public int Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged(nameof(Order)); }
        }
        public int Id => user.Id;
        public string? username => user.UserName;
        public string? Role { get; set; }
        public string? Position => user.Position;
        public string? OwnerName => user.OwnerName;
        public UserListItemViewModel(User user, NavigationStore navigationStore, UsersDataStore usersDataStore, UsersListViewModel usersListViewModel, EmployeeStore employeeStore, AuthenticationStore authenticationStore)
        {
            this.user = user;
            switch (user.Role)
            {
                case Core.Common.Roles.Admin:
                    Role = "مسؤول";
                    break;
                case Core.Common.Roles.Supervisor:
                    Role = "مشرف";
                    break;
                case Core.Common.Roles.User:
                    Role = "مستخدم";
                    break;
                case Core.Common.Roles.Accountant:
                    Role = "محاسب";
                    break;
            }
            _navigationStore = navigationStore;
            _usersDataStore = usersDataStore;
            _authenticationStore = authenticationStore;
            _usersListViewModel = usersListViewModel;
            _employeeStore = employeeStore;
            EditUserCommand = new NavaigateCommand<EditUserViewModel>(new NavigationService<EditUserViewModel>(_navigationStore, () => CreateEditPlayerViewModel(_usersDataStore, _navigationStore, _usersListViewModel, _employeeStore)));
            DeleteUserCommand = new DeleteUserCommand(_usersDataStore, _authenticationStore);
        }

        private EditUserViewModel CreateEditPlayerViewModel(UsersDataStore usersDataStore, NavigationStore navigatorStore, UsersListViewModel usersListViewModel, EmployeeStore employeeStore)
        {
            return EditUserViewModel.loadViewModel(usersDataStore, navigatorStore, usersListViewModel, employeeStore);
        }


        public void update(User user)
        {
            this.user = user;
        }
        public ICommand EditUserCommand { get; }
        public ICommand? DeleteUserCommand { get; }
    }
}
