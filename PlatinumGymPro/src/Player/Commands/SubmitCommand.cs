using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores.PlayerStores;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands
{
    public class SubmitCommand : AsyncCommandBase
    {

        private readonly NavigationService<PlayerListViewModel> navigationService;
        private readonly PlayerStore _playerStore;
        private AddPlayerViewModel _addPlayerViewModel;
        public SubmitCommand(NavigationService<PlayerListViewModel> navigationService, PlayerStore playerStore,AddPlayerViewModel addPlayerViewModel)
        {

            this.navigationService = navigationService;
            _playerStore = playerStore;
            _addPlayerViewModel = addPlayerViewModel;
        }
        public override bool CanExecute(object? parameter)
        {
            return  base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
           
            Player player = new Player()
            {
                FullName = _addPlayerViewModel.FullName,
                Balance = _addPlayerViewModel.Balance,
                BirthDate = _addPlayerViewModel.BirthDate,
                GenderMale = _addPlayerViewModel.GenderMale,
                Hieght = _addPlayerViewModel.Hieght,
                IsSubscribed = _addPlayerViewModel.IsSubscribed,
                IsTakenContainer = _addPlayerViewModel.IsTakenContainer,
                Phone = _addPlayerViewModel.Phone,
                SubscribeDate = _addPlayerViewModel.SubscribeDate,
                SubscribeEndDate = _addPlayerViewModel.SubscribeDate.AddDays(30),
                Weight = _addPlayerViewModel.Weight,
            };
            await _playerStore.Add(player);
            MessageBox.Show(player.FullName + " added successfully");
            navigationService.Navigate();
        }

        
    }
}
