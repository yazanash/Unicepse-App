using PlatinumGym.Core.Models.Subscription;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.PaymentsViewModels;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using PlatinumGymPro.ViewModels.SubscriptionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands.SubscriptionCommand
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
                Subscription subscription = new()
                {
                    /// subscription info
                    Id = _subscriptionDataStore.SelectedSubscription!.Id,
                    Sport = _subscriptionDataStore.SelectedSport,
                    DaysCount = _editSubscriptionViewModel.SubscribeDays,
                    LastCheck = _editSubscriptionViewModel.SubscribeDate,
                    Trainer = _subscriptionDataStore.SelectedTrainer,
                    TrainerId = _subscriptionDataStore.SelectedTrainer != null ? _subscriptionDataStore.SelectedTrainer.Id : null,
                    Player = _playerDataStore.SelectedPlayer!.Player,
                    RollDate = _editSubscriptionViewModel.SubscribeDate,
                    Price = _subscriptionDataStore.SelectedSport!.Price,
                    /// offer info
                    OfferValue = _editSubscriptionViewModel.OfferValue,
                    OfferDes = _editSubscriptionViewModel.Offer,
                    PriceAfterOffer = _subscriptionDataStore.SelectedSport.Price - _editSubscriptionViewModel.OfferValue,
                    /// private info
                    IsPrivate = _editSubscriptionViewModel.PrivatePrice > 0,
                    PrivatePrice = _editSubscriptionViewModel.PrivatePrice,
                    IsPlayerPay = _editSubscriptionViewModel.PrivateProvider,
                    EndDate = _editSubscriptionViewModel.SubscribeDate.AddDays(_editSubscriptionViewModel.SubscribeDays),
                };

                await _subscriptionDataStore.Update(subscription);

                _navigationService.Navigate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
