using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.Core.Exceptions;
using Uniceps.Stores;
using Uniceps.navigation;
using Uniceps.ViewModels.PlayersViewModels;

namespace Uniceps.Commands.Player
{
    public class ReactivePlayerCommand : AsyncCommandBase
    {
        private readonly PlayersDataStore _playerStore;
        public ReactivePlayerCommand(PlayersDataStore playerStore)
        {
            _playerStore = playerStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                if (parameter != null)
                {
                    int playerId = Convert.ToInt32(parameter);
                    if (playerId > 0)
                    {
                        Core.Models.Player.Player player = _playerStore.ArchivedPlayers.FirstOrDefault(x=>x.Id == playerId)!;
                        player.IsSubscribed = true;
                        await _playerStore.ReactivePlayer(player);
                    }
                }
                
            }
            catch (NotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
