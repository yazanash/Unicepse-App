//using PlatinumGymPro.Models;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.ViewModels.PlayersViewModels;
using Unicepse.Commands;
using Unicepse.navigation.Stores;
using Unicepse.navigation;

namespace Unicepse.Commands.Player
{
    public class SubmitCommand : AsyncCommandBase
    {

        private readonly NavigationService<PlayerProfileViewModel> navigationService;
        private readonly PlayersDataStore _playerStore;
        private readonly AddPlayerViewModel _addPlayerViewModel;
        public SubmitCommand(NavigationService<PlayerProfileViewModel> navigationService, AddPlayerViewModel addPlayerViewModel, PlayersDataStore playerStore)
        {

            this.navigationService = navigationService;
            _playerStore = playerStore;
            _addPlayerViewModel = addPlayerViewModel;
            _addPlayerViewModel.PropertyChanged += AddPlayerViewModel_PropertyChanged;
        }

        private void AddPlayerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_addPlayerViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {

            return _addPlayerViewModel.CanSubmit && !string.IsNullOrEmpty(_addPlayerViewModel.FullName) && _addPlayerViewModel.Phone!.Trim().Length > 9 && base.CanExecute(null);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {


                _addPlayerViewModel.Submited = false;
                Core.Models.Player.Player player = new()
                {
                    FullName = _addPlayerViewModel.FullName,
                    BirthDate = _addPlayerViewModel.Year!.year,
                    GenderMale = _addPlayerViewModel.GenderMale,
                    Hieght = _addPlayerViewModel.Hieght,
                    Phone = _addPlayerViewModel.Phone,
                    SubscribeDate = _addPlayerViewModel.SubscribeDate,
                    SubscribeEndDate = _addPlayerViewModel.SubscribeDate.AddDays(30),
                    Weight = _addPlayerViewModel.Weight,
                    IsSubscribed = true,
                };
                await _playerStore.AddPlayer(player);
                _playerStore.SelectedPlayer = player;
                if (!string.IsNullOrEmpty(_addPlayerViewModel.UID))
                    await _playerStore.HandShakePlayer(player, _addPlayerViewModel.UID!);
                _addPlayerViewModel.Submited = true;
                navigationService.ReNavigate();
            }
            catch (PlayerConflictException ex)
            {
                _addPlayerViewModel.ClearError(nameof(_addPlayerViewModel.FullName));
                _addPlayerViewModel.AddError(ex.Message, nameof(_addPlayerViewModel.FullName));
                _addPlayerViewModel.OnErrorChanged(nameof(_addPlayerViewModel.FullName));
            }
        }


    }
}
