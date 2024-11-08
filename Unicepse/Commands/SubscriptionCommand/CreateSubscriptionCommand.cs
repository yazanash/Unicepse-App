using Unicepse.Core.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.ViewModels.PaymentsViewModels;
using Unicepse.ViewModels.SubscriptionViewModel;
using Unicepse.navigation;

namespace Unicepse.Commands.SubscriptionCommand
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

            return _addSubscriptionViewModel.CanSubmit && _addSubscriptionViewModel.SelectedSport != null && base.CanExecute(null);
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
                    Player = _playerDataStore.SelectedPlayer!,
                    RollDate = _addSubscriptionViewModel.SubscribeDate,
                    //Price = _subscriptionDataStore.SelectedSport!.Price,
                    LastPaid = _addSubscriptionViewModel.SubscribeDate,
                    /// offer info
                    OfferValue = _addSubscriptionViewModel.OfferValue,
                    OfferDes = _addSubscriptionViewModel.Offer,
                   
                    /// private info
                    IsPrivate = _addSubscriptionViewModel.PrivatePrice > 0,
                    IsPlayerPay = _addSubscriptionViewModel.PrivateProvider,
                    PrivatePrice = _addSubscriptionViewModel.PrivatePrice,
                    EndDate = _addSubscriptionViewModel.SubscribeDate.AddDays(_addSubscriptionViewModel.SubscribeDays),
                };
                if (_addSubscriptionViewModel.DaysCounter)
                {
                    subscription.Price = _subscriptionDataStore.SelectedSport!.Price;
                    subscription.PriceAfterOffer = subscription.Price - _addSubscriptionViewModel.OfferValue;
                }
                else
                {
                    subscription.Price = _subscriptionDataStore.SelectedSport!.DailyPrice * _addSubscriptionViewModel.SubscribeDays;
                    subscription.PriceAfterOffer = subscription.Price - _addSubscriptionViewModel.OfferValue;
                }
                await _subscriptionDataStore.Add(subscription);
                _playerDataStore.SelectedPlayer!.Balance -= subscription.PriceAfterOffer;
                _playerDataStore.SelectedPlayer!.SubscribeEndDate = subscription.EndDate;
                _playerDataStore.SelectedPlayer!.IsSubscribed = true;
                await _playerDataStore.UpdatePlayer(_playerDataStore.SelectedPlayer!);
                _navigationService.ReNavigate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
