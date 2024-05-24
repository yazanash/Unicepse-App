using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.WPF.navigation.Stores;
using Unicepse.WPF.navigation;

namespace Unicepse.WPF.Commands.Player
{
    public class EditPlayerCommand : AsyncCommandBase
    {
        private readonly NavigationService<PlayerMainPageViewModel> navigationService;
        private readonly NavigationStore _navigationStore;
        private readonly PlayersDataStore _playerStore;
        private readonly SportDataStore _sportStore;
        private readonly EditPlayerViewModel _editPlayerViewModel;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        public EditPlayerCommand(NavigationService<PlayerMainPageViewModel> navigationService, EditPlayerViewModel editPlayerViewModel, PlayersDataStore playerStore, NavigationStore navigationStore, SubscriptionDataStore subscriptionDataStore, SportDataStore sportStore)
        {

            this.navigationService = navigationService;
            _playerStore = playerStore;
            _editPlayerViewModel = editPlayerViewModel;
            _editPlayerViewModel.PropertyChanged += AddPlayerViewModel_PropertyChanged;
            _navigationStore = navigationStore;
            _subscriptionDataStore = subscriptionDataStore;
            _sportStore = sportStore;
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

            return _editPlayerViewModel.CanSubmit && !string.IsNullOrEmpty(_editPlayerViewModel.FullName) &&_editPlayerViewModel.Phone!.Trim().Length > 9 && base.CanExecute(null);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                _editPlayerViewModel.Submited = false;
                Unicepse.Core.Models.Player.Player player = _playerStore.SelectedPlayer!.Player;

                player.FullName = _editPlayerViewModel.FullName;
                player.BirthDate = _editPlayerViewModel.Year!.year;
                player.GenderMale = _editPlayerViewModel.GenderMale;
                player.Hieght = _editPlayerViewModel.Hieght;
                player.Phone = _editPlayerViewModel.Phone;
                player.SubscribeDate = _editPlayerViewModel.SubscribeDate;
                player.SubscribeEndDate = _editPlayerViewModel.SubscribeDate.AddDays(30);
                player.Weight = _editPlayerViewModel.Weight;

                await _playerStore.UpdatePlayer(player);
                MessageBox.Show(player.FullName + " edited successfully");
                navigationService.Navigate();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          
           
        }

    }
}
