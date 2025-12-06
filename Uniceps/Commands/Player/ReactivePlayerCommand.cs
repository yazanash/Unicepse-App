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
