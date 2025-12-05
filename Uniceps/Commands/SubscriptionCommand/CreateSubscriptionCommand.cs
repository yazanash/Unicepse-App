using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.ViewModels.PaymentsViewModels;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.Commands.SubscriptionCommand
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
                    SportId = _subscriptionDataStore.SelectedSport!.Id,
                    SportName = _subscriptionDataStore.SelectedSport!.Name,
                    LastCheck = _addSubscriptionViewModel.SubscribeDate,
                    TrainerId = _subscriptionDataStore.SelectedTrainer?.Id,
                    TrainerName = _subscriptionDataStore.SelectedSport!.Name,
                    PlayerId = _playerDataStore.SelectedPlayer!.Id,
                    PlayerName = _subscriptionDataStore.SelectedSport!.Name,
                    RollDate = _addSubscriptionViewModel.SubscribeDate,
                    Price = _subscriptionDataStore.SelectedSport!.Price,
                    OfferValue = _addSubscriptionViewModel.OfferValue,
                    OfferDes = _addSubscriptionViewModel.Offer,
                    EndDate = _addSubscriptionViewModel.SubscribeDate.AddDays(_addSubscriptionViewModel.SubscribeDays),
                    PriceAfterOffer = _addSubscriptionViewModel.Total ?? 0
                };
                await _subscriptionDataStore.Add(subscription);
                _playerDataStore.SelectedPlayer!.Balance -= subscription.PriceAfterOffer;
                Subscription? subscriptions = _subscriptionDataStore.Subscriptions.OrderByDescending(x => x.EndDate).FirstOrDefault(x => x.Id != subscription.Id);
                if (subscriptions != null && subscriptions.EndDate >= subscription.EndDate)
                {
                    _playerDataStore.SelectedPlayer!.SubscribeEndDate = subscriptions.EndDate;
                }
                else
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
