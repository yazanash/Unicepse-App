using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.AuthCommands;
using Uniceps.Commands.Player;
using Uniceps.Core.Models.Authentication;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.Views.AuthView;

namespace Uniceps.ViewModels.Authentication
{
    public class UserListItemViewModel : ViewModelBase
    {
        public User user;
        private readonly UsersDataStore _usersDataStore;
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
        public UserListItemViewModel(User user, UsersDataStore usersDataStore,AuthenticationStore authenticationStore)
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
            _usersDataStore = usersDataStore;
            _authenticationStore = authenticationStore;
            EditUserCommand = new RelayCommand(ExecuteEditUserCommand);
            DeleteUserCommand = new DeleteUserCommand(_usersDataStore, _authenticationStore);
        }

        private void ExecuteEditUserCommand()
        {
            EditUserViewModel addUserViewModel  = new  EditUserViewModel(_usersDataStore,this);
            AddUserViewWindow addUserViewWindow = new AddUserViewWindow();
            addUserViewWindow.DataContext = addUserViewModel;
            addUserViewWindow.ShowDialog();
        }
        public void update(User user)
        {
            this.user = user;
        }
        public ICommand EditUserCommand { get; }
        public ICommand? DeleteUserCommand { get; }
    }
}
