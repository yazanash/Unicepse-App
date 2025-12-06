using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.navigation.Stores;
using Uniceps.ViewModels.PlayersViewModels;

namespace Uniceps.Commands.Player
{
    public class EditPlayerCommand : AsyncCommandBase
    {
        private readonly PlayersDataStore _playerStore;
        private readonly EditPlayerViewModel _editPlayerViewModel;
        public EditPlayerCommand( EditPlayerViewModel editPlayerViewModel, PlayersDataStore playerStore)
        {
            _playerStore = playerStore;
            _editPlayerViewModel = editPlayerViewModel;
            _editPlayerViewModel.PropertyChanged += AddPlayerViewModel_PropertyChanged;
        }

        private void AddPlayerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_editPlayerViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {

            return _editPlayerViewModel.CanSubmit && !string.IsNullOrEmpty(_editPlayerViewModel.FullName) && _editPlayerViewModel.Phone!.Trim().Length > 9 && base.CanExecute(null);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                _editPlayerViewModel.Submited = false;
                Core.Models.Player.Player player = _playerStore.SelectedPlayer!;

                player.FullName = _editPlayerViewModel.FullName;
                player.BirthDate = _editPlayerViewModel.Year?.year ?? DateTime.Now.Year;
                player.GenderMale = _editPlayerViewModel.GenderMale;
                player.Hieght = _editPlayerViewModel.Hieght;
                player.Phone = _editPlayerViewModel.Phone;
                player.SubscribeDate = _editPlayerViewModel.SubscribeDate;
                player.SubscribeEndDate = _editPlayerViewModel.SubscribeDate.AddDays(30);
                player.Weight = _editPlayerViewModel.Weight;

                await _playerStore.UpdatePlayer(player);
                _editPlayerViewModel.OnPlayerUpdated();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

    }
}
