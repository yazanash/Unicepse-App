using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Stores;

namespace Uniceps.Commands.Player
{
    public class LoadAllPlayersCommand : AsyncCommandBase
    {
        private readonly PlayersDataStore _playersDataStore;

        public LoadAllPlayersCommand(PlayersDataStore playersDataStore)
        {
            _playersDataStore = playersDataStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            await _playersDataStore.GetAll();
        }
    }
}
