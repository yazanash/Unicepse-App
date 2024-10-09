using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;

namespace Unicepse.Commands.AuthCommands
{
    public class DeleteUserCommand : AsyncCommandBase
    {
        private readonly UsersDataStore _usersDataStore;
        private readonly AuthenticationStore _authenticationStore;
        public DeleteUserCommand(UsersDataStore usersDataStore, AuthenticationStore authenticationStore)
        {
            _usersDataStore = usersDataStore;
            _authenticationStore = authenticationStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                if (_authenticationStore.CurrentAccount!.Id != _usersDataStore.SelectedUser!.Id)
                    await _usersDataStore.Delete(_usersDataStore.SelectedUser!.Id);
                else
                    MessageBox.Show("لا يمكن حذف المستخدم الحالي");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
