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
    public class SubmitUserCommand : AsyncCommandBase
    {
        private readonly UsersDataStore _usersDataStore;
        private NavigationService<UsersListViewModel> _navigationService;
        private AddUserViewModel _addUserViewModel;
        public SubmitUserCommand(UsersDataStore usersDataStore, NavigationService<UsersListViewModel> navigationService, AddUserViewModel addUserViewModel)
        {
            _usersDataStore = usersDataStore;
            _navigationService = navigationService;
            _addUserViewModel = addUserViewModel;
            _addUserViewModel.PropertyChanged += _addUserViewModel_PropertyChanged;
        }

        private void _addUserViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_addUserViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }
        public override bool CanExecute(object? parameter)
        {

            return _addUserViewModel.CanSubmit && !string.IsNullOrEmpty(_addUserViewModel.Password)
                && !string.IsNullOrEmpty(_addUserViewModel.UserName)
                && !string.IsNullOrEmpty(_addUserViewModel.OwnerName)
                && !string.IsNullOrEmpty(_addUserViewModel.Position);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                User user = new User()
                {
                    UserName = _addUserViewModel.UserName,
                    Password = _addUserViewModel.Password,
                    Role = _addUserViewModel.RoleItem!.role,
                    Position = _addUserViewModel.Position,
                    OwnerName = _addUserViewModel.OwnerName
                };
                await _usersDataStore.Add(user);
                _navigationService.ReNavigate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
