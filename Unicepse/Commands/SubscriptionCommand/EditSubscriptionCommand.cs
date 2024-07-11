﻿using Unicepse.Core.Models.Subscription;
using Unicepse.ViewModels.PaymentsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.ViewModels.SubscriptionViewModel;
using Unicepse.ViewModels.PlayersViewModels;
using Unicepse.navigation;

namespace Unicepse.Commands.SubscriptionCommand
{
    public class EditSubscriptionCommand : AsyncCommandBase
    {
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly EditSubscriptionViewModel _editSubscriptionViewModel;
        private readonly PlayersDataStore _playerDataStore;
        private readonly NavigationService<PlayerMainPageViewModel> _navigationService;

        public EditSubscriptionCommand(SubscriptionDataStore subscriptionDataStore, EditSubscriptionViewModel editSubscriptionViewModel, PlayersDataStore playerDataStore, NavigationService<PlayerMainPageViewModel> navigationService)
        {
            _subscriptionDataStore = subscriptionDataStore;
            _editSubscriptionViewModel = editSubscriptionViewModel;
            _playerDataStore = playerDataStore;
            _navigationService = navigationService;
            _editSubscriptionViewModel.PropertyChanged += _editSubscriptionViewModel_PropertyChanged;
        }

        private void _editSubscriptionViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_editSubscriptionViewModel.CanSubmit))
            {
                OnCanExecutedChanged();
            }
        }

        public override bool CanExecute(object? parameter)
        {

            return _editSubscriptionViewModel.CanSubmit && _editSubscriptionViewModel.SelectedSport != null && base.CanExecute(null);
        }
        public async override Task ExecuteAsync(object? parameter)
        {
            try
            {
                _playerDataStore.SelectedPlayer!.Player.Balance += _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer;
                /// subscription info
                _subscriptionDataStore.SelectedSubscription!.Id = _subscriptionDataStore.SelectedSubscription!.Id;
                _subscriptionDataStore.SelectedSubscription!.Sport = _subscriptionDataStore.SelectedSport;
                _subscriptionDataStore.SelectedSubscription!.DaysCount = _editSubscriptionViewModel.SubscribeDays;
                _subscriptionDataStore.SelectedSubscription!.LastCheck = _editSubscriptionViewModel.SubscribeDate;
                _subscriptionDataStore.SelectedSubscription!.Trainer = _subscriptionDataStore.SelectedTrainer;
                _subscriptionDataStore.SelectedSubscription!.TrainerId = _subscriptionDataStore.SelectedTrainer != null ? _subscriptionDataStore.SelectedTrainer.Id : null;
                _subscriptionDataStore.SelectedSubscription!.Player = _playerDataStore.SelectedPlayer!.Player;
                _subscriptionDataStore.SelectedSubscription!.RollDate = _editSubscriptionViewModel.SubscribeDate;
                _subscriptionDataStore.SelectedSubscription!.Price = _subscriptionDataStore.SelectedSport!.Price;
                /// offer info
                _subscriptionDataStore.SelectedSubscription!.OfferValue = _editSubscriptionViewModel.OfferValue;
                _subscriptionDataStore.SelectedSubscription!.OfferDes = _editSubscriptionViewModel.Offer;
                _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer = _subscriptionDataStore.SelectedSport.Price - _editSubscriptionViewModel.OfferValue;
                /// private info
                _subscriptionDataStore.SelectedSubscription!.IsPrivate = _editSubscriptionViewModel.PrivatePrice > 0;
                _subscriptionDataStore.SelectedSubscription!.PrivatePrice = _editSubscriptionViewModel.PrivatePrice;
                _subscriptionDataStore.SelectedSubscription!.IsPlayerPay = _editSubscriptionViewModel.PrivateProvider;
                _subscriptionDataStore.SelectedSubscription!.EndDate = _editSubscriptionViewModel.SubscribeDate.AddDays(_editSubscriptionViewModel.SubscribeDays);

                _subscriptionDataStore.SelectedSubscription!.PaidValue = _subscriptionDataStore.SelectedSubscription!.PaidValue;
                if (_subscriptionDataStore.SelectedSubscription!.PaidValue == _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer)
                    _subscriptionDataStore.SelectedSubscription!.IsPaid = true;
                else
                    _subscriptionDataStore.SelectedSubscription!.IsPaid = false;
                _playerDataStore.SelectedPlayer!.Player.Balance -= _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer;
                await _subscriptionDataStore.Update(_subscriptionDataStore.SelectedSubscription!);
                await _playerDataStore.UpdatePlayer(_playerDataStore.SelectedPlayer!.Player);
                _navigationService.Navigate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
