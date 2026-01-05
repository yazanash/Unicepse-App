using Uniceps.ViewModels.PaymentsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.Commands.SubscriptionCommand
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
                _playerDataStore.SelectedPlayer!.Balance += _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer;
                /// subscription info
                _subscriptionDataStore.SelectedSubscription!.Id = _subscriptionDataStore.SelectedSubscription!.Id;
                _subscriptionDataStore.SelectedSubscription!.SportId = _subscriptionDataStore.SelectedSport!.Id;
                _subscriptionDataStore.SelectedSubscription!.SportSyncId = _subscriptionDataStore.SelectedSport!.SyncId;
                _subscriptionDataStore.SelectedSubscription!.SportName = _subscriptionDataStore.SelectedSport.Name;
                _subscriptionDataStore.SelectedSubscription!.DaysCount = _editSubscriptionViewModel.SubscribeDays;
                _subscriptionDataStore.SelectedSubscription!.LastCheck = _editSubscriptionViewModel.SubscribeDate;
                _subscriptionDataStore.SelectedSubscription!.TrainerName = _subscriptionDataStore.SelectedTrainer?.FullName;
                _subscriptionDataStore.SelectedSubscription!.TrainerId = _subscriptionDataStore.SelectedTrainer != null ? _subscriptionDataStore.SelectedTrainer.Id : null;
                _subscriptionDataStore.SelectedSubscription!.TrainerSyncId = _subscriptionDataStore.SelectedTrainer != null ? _subscriptionDataStore.SelectedTrainer.SyncId : null;
                _subscriptionDataStore.SelectedSubscription!.PlayerId = _playerDataStore.SelectedPlayer!.Id;
                _subscriptionDataStore.SelectedSubscription!.PlayerSyncId = _playerDataStore.SelectedPlayer!.SyncId;
                _subscriptionDataStore.SelectedSubscription!.PlayerName = _playerDataStore.SelectedPlayer!.FullName;
                _subscriptionDataStore.SelectedSubscription!.RollDate = _editSubscriptionViewModel.SubscribeDate;
                _subscriptionDataStore.SelectedSubscription!.Price = _subscriptionDataStore.SelectedSport!.Price;
                /// offer info
                _subscriptionDataStore.SelectedSubscription!.OfferValue = _editSubscriptionViewModel.OfferValue;
                _subscriptionDataStore.SelectedSubscription!.OfferDes = _editSubscriptionViewModel.Offer;
                //_subscriptionDataStore.SelectedSubscription!.PriceAfterOffer = _subscriptionDataStore.SelectedSport.Price - _editSubscriptionViewModel.OfferValue;
                /// private info
                _subscriptionDataStore.SelectedSubscription!.EndDate = _editSubscriptionViewModel.SubscribeDate.AddDays(_editSubscriptionViewModel.SubscribeDays);
                _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer = _editSubscriptionViewModel.Total ?? 0;

                _playerDataStore.SelectedPlayer!.Balance -= _subscriptionDataStore.SelectedSubscription!.PriceAfterOffer;
                if (_playerDataStore.SelectedPlayer!.SubscribeEndDate < _subscriptionDataStore.SelectedSubscription!.EndDate)
                {
                    _playerDataStore.SelectedPlayer!.SubscribeEndDate = _subscriptionDataStore.SelectedSubscription!.EndDate;
                }
                await _subscriptionDataStore.Update(_subscriptionDataStore.SelectedSubscription!);
                Subscription? subscription = _subscriptionDataStore.Subscriptions.OrderByDescending(x => x.EndDate).FirstOrDefault(x => x.Id != _subscriptionDataStore.SelectedSubscription.Id);
                if (subscription != null && subscription.EndDate >= _subscriptionDataStore.SelectedSubscription.EndDate)
                {
                    _playerDataStore.SelectedPlayer!.SubscribeEndDate = subscription.EndDate;
                }
                else
                    _playerDataStore.SelectedPlayer!.SubscribeEndDate = _subscriptionDataStore.SelectedSubscription.EndDate;
                await _playerDataStore.UpdatePlayer(_playerDataStore.SelectedPlayer!);
                _navigationService.Navigate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
