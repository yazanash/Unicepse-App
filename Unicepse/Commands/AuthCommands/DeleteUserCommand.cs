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
        public DeleteUserCommand(UsersDataStore usersDataStore)
        {
            _usersDataStore = usersDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                await _usersDataStore.Delete(_usersDataStore.SelectedUser!.Id);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
