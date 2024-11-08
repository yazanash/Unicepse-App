using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Core.Exceptions;
using Unicepse.navigation;
using Unicepse.Stores;
using Unicepse.ViewModels.PlayersViewModels;

namespace Unicepse.Commands.Player
{
    public class ReactivePlayerCommand : AsyncCommandBase
    {
        private readonly PlayersDataStore _playerStore;
        public ReactivePlayerCommand( PlayersDataStore playerStore)
        {
            _playerStore = playerStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                Core.Models.Player.Player player = _playerStore.SelectedPlayer!;
                player.IsSubscribed = true;
                await _playerStore.ReactivePlayer(player);
            }
            catch (NotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
