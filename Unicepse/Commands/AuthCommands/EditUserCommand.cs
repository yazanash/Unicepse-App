using Unicepse.Core.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.ViewModels.Authentication;
using Unicepse.Stores;
using Unicepse.navigation;
using System.Windows;

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
            _editUserViewModel.PropertyChanged += _addUserViewModel_PropertyChanged;
        }
        private void _addUserViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_editUserViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }
        public override bool CanExecute(object? parameter)
        {

            return _editUserViewModel.CanSubmit && !string.IsNullOrEmpty(_editUserViewModel.Password)
                && !string.IsNullOrEmpty(_editUserViewModel.UserName)
                && !string.IsNullOrEmpty(_editUserViewModel.OwnerName)
                && !string.IsNullOrEmpty(_editUserViewModel.Position);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                User user = new User()
                {
                    Id = _usersDataStore.SelectedUser!.Id,
                    UserName = _editUserViewModel.UserName,
                    Password = _editUserViewModel.Password,
                    Role = _editUserViewModel.RoleItem!.role,
                    Position = _editUserViewModel.Position,
                    OwnerName = _editUserViewModel.OwnerName
                };
                await _usersDataStore.Update(user);
                _navigationService.ReNavigate();
            }
           catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
