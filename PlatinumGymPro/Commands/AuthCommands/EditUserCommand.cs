using PlatinumGym.Core.Models.Authentication;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.AuthCommands
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
                Id=_usersDataStore.SelectedUser!.Id,
                Employee = _usersDataStore.SelectedEmployee,
                UserName = _editUserViewModel.UserName,
                Password = _editUserViewModel.Password,

            };
            await _usersDataStore.Add(user);
            _navigationService.ReNavigate();
        }
    }
}
