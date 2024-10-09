using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.ViewModels.Authentication;

namespace Unicepse.Commands.AuthCommands
{
    internal class LoadLogsCommand : AsyncCommandBase
    {
        private readonly UsersDataStore _usersDataStore;
        private readonly AuthenticationLoggingList _authenticationLoggingList;
        public LoadLogsCommand(UsersDataStore usersDataStore, AuthenticationLoggingList authenticationLoggingList)
        {
            _usersDataStore = usersDataStore;
            _authenticationLoggingList = authenticationLoggingList;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _usersDataStore.GetAuthenticationLog(_authenticationLoggingList.Date);
        }
    }
}
