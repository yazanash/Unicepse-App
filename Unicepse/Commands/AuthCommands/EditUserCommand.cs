using Unicepse.Core.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.ViewModels.Authentication;
using Unicepse.Stores;
using Unicepse.navigation;

namespace Unicepse.Commands.AuthCommands
{
    public class EditUserCommand : AsyncCommandBase
    {
        private readonly UsersDataStore _usersDataStore;
        private NavigationService<UsersListViewModel> _navigationService;
        private EditUserViewModel _editUserViewModel;
        public EditUserCommand(UsersDataStore usersDataStore, NavigationService<UsersListViewModel> navigationService, EditUserViewModel editUserViewModel)
        {
            _usersDataStore = usersDataStore;
            _navigationService = navigationService;
            _editUserViewModel = editUserViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            User user = new User()
            {
                Id = _usersDataStore.SelectedUser!.Id,
                Employee = _usersDataStore.SelectedEmployee,
                UserName = _editUserViewModel.UserName,
                Password = _editUserViewModel.Password,

            };
            await _usersDataStore.Update(user);
            _navigationService.ReNavigate();
        }
    }
}
