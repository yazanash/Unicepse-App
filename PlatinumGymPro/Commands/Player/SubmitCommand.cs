//using PlatinumGymPro.Models;
using PlatinumGym.Core.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
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

        private readonly NavigationService<PlayerProfileViewModel> navigationService;
        private readonly NavigationStore _navigationStore;
        private readonly PlayersDataStore _playerStore;
        private readonly SportDataStore _sportStore;
        private readonly PlayerListViewModel _PlayerListViewModel;
        private readonly AddPlayerViewModel _addPlayerViewModel;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly MetricDataStore _metricDataStore;
        private readonly RoutineDataStore _routineDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        public SubmitCommand(NavigationService<PlayerProfileViewModel> navigationService, AddPlayerViewModel addPlayerViewModel, PlayersDataStore playerStore, NavigationStore navigationStore, PlayerListViewModel playerListViewModel, SubscriptionDataStore subscriptionDataStore, SportDataStore sportStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, PaymentDataStore paymentDataStore)
        {

            this.navigationService = navigationService;
            _playerStore = playerStore;
            _addPlayerViewModel = addPlayerViewModel;
            _navigationStore = navigationStore;
            _addPlayerViewModel.PropertyChanged += AddPlayerViewModel_PropertyChanged;
            _PlayerListViewModel = playerListViewModel;
            _subscriptionDataStore = subscriptionDataStore;
            _sportStore = sportStore;
            _metricDataStore = metricDataStore;
            _routineDataStore = routineDataStore;
            _paymentDataStore = paymentDataStore;
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

            return  _addPlayerViewModel.CanSubmit && base.CanExecute(null) ;
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            _addPlayerViewModel.Submited = false;
              PlatinumGym.Core.Models.Player.Player player = new ()
            {
                FullName = _addPlayerViewModel.FullName,
                BirthDate = _addPlayerViewModel.BirthDate,
                GenderMale = _addPlayerViewModel.GenderMale,
                Hieght = _addPlayerViewModel.Hieght,
                Phone = _addPlayerViewModel.Phone,
                SubscribeDate = _addPlayerViewModel.SubscribeDate,
                SubscribeEndDate = _addPlayerViewModel.SubscribeDate.AddDays(30),
                Weight = _addPlayerViewModel.Weight,
            };
            await _playerStore.AddPlayer(player);
            _playerStore.SelectedPlayer = new PlayerListItemViewModel(player, _navigationStore, _subscriptionDataStore, _playerStore, _sportStore,_paymentDataStore, _metricDataStore, _routineDataStore, _PlayerListViewModel);
            MessageBox.Show(player.FullName + " added successfully");
            //_addPlayerViewModel.Submited = true;
            //_addPlayerViewModel.SubmitMessage = player.FullName + " added successfully";
            //_playerStore.SelectedPlayer = new PlayerListItemViewModel(player, _navigationStore,_subscriptionDataStore, _playerStore,_sportStore);
           //await Task.Delay(5000);
            navigationService.ReNavigate();
        }

        
    }
}
