using Unicepse.Core.Models.Subscription;
using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.PaymentsViewModels;
using Unicepse.WPF.ViewModels.SubscriptionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.WPF.navigation;

namespace Unicepse.WPF.Commands.SubscriptionCommand
{
    public class CreateSubscriptionCommand : AsyncCommandBase
    {
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly SubscriptionDetailsViewModel _addSubscriptionViewModel;
        private readonly PlayersDataStore _playerDataStore;
        private readonly NavigationService<AddPaymentViewModel> _navigationService;

        public CreateSubscriptionCommand(SubscriptionDataStore subscriptionDataStore, SubscriptionDetailsViewModel addSubscriptionViewModel, PlayersDataStore playerDataStore, NavigationService<AddPaymentViewModel> navigationService)
        {
            _subscriptionDataStore = subscriptionDataStore;
            _addSubscriptionViewModel = addSubscriptionViewModel;
            _playerDataStore = playerDataStore;
            _navigationService = navigationService;
            _addSubscriptionViewModel.PropertyChanged += _addSubscriptionViewModel_PropertyChanged;
        }

        private void _addSubscriptionViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_addSubscriptionViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }
        public override bool CanExecute(object? parameter)
        {

            return _addSubscriptionViewModel.CanSubmit && _addSubscriptionViewModel.SelectedSport!=null && base.CanExecute(null);
        }
        public async override Task ExecuteAsync(object? parameter)
        {
            try
            {
                Subscription subscription = new()
                {
                    /// subscription info
                    DaysCount = _addSubscriptionViewModel.SubscribeDays,
                    Sport = _subscriptionDataStore.SelectedSport,
                    LastCheck = _addSubscriptionViewModel.SubscribeDate,
                    Trainer = _subscriptionDataStore.SelectedTrainer,
                    Player = _playerDataStore.SelectedPlayer!.Player,
                    RollDate = _addSubscriptionViewModel.SubscribeDate,
                    Price = _subscriptionDataStore.SelectedSport!.Price,
                    LastPaid = _addSubscriptionViewModel.SubscribeDate,
                    /// offer info
                    OfferValue = _addSubscriptionViewModel.OfferValue,
                    OfferDes = _addSubscriptionViewModel.Offer,
                    PriceAfterOffer = _subscriptionDataStore.SelectedSport.Price - _addSubscriptionViewModel.OfferValue,
                    /// private info
                    IsPrivate = _addSubscriptionViewModel.PrivatePrice > 0,
                    IsPlayerPay = _addSubscriptionViewModel.PrivateProvider,
                    PrivatePrice = _addSubscriptionViewModel.PrivatePrice,
                    EndDate = _addSubscriptionViewModel.SubscribeDate.AddDays(_addSubscriptionViewModel.SubscribeDays),
                };
                _playerDataStore.SelectedPlayer!.Player.Balance -= subscription.PriceAfterOffer;
                await _subscriptionDataStore.Add(subscription);
                await _playerDataStore.UpdatePlayer(_playerDataStore.SelectedPlayer!.Player);
                _navigationService.ReNavigate();
            }
           catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
