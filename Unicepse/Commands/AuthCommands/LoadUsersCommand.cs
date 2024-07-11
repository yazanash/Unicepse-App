using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;

namespace Unicepse.Commands.AuthCommands
{
    public class LoadUsersCommand : AsyncCommandBase
    {
        private readonly UsersDataStore _usersDataStore;

        public LoadUsersCommand(UsersDataStore usersDataStore)
        {
            _usersDataStore = usersDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _usersDataStore.GetAll();
        }
    }
}
