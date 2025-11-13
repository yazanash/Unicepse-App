using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Subscription;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.ViewModels.PaymentsViewModels;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using playerModel = Uniceps.Core.Models.Player;

namespace Uniceps.Commands.SubscriptionCommand
{
    public class CreateMainSubscriptionCommand : AsyncCommandBase
    {
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly CreateSubscriptionWindowViewModel _addSubscriptionViewModel;
        private readonly PlayersDataStore _playerDataStore;

        public CreateMainSubscriptionCommand(SubscriptionDataStore subscriptionDataStore, CreateSubscriptionWindowViewModel addSubscriptionViewModel, PlayersDataStore playerDataStore)
        {
            _subscriptionDataStore = subscriptionDataStore;
            _addSubscriptionViewModel = addSubscriptionViewModel;
            _playerDataStore = playerDataStore;
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
                playerModel.Player? player = null;
                if (_addSubscriptionViewModel.SelectedPlayer != null)
                {
                    player = _addSubscriptionViewModel.SelectedPlayer.Player;
                }
                else
                {
                    player = new playerModel.Player()
                    {
                        FullName = _addSubscriptionViewModel.PlayerName,
                        BirthDate = _addSubscriptionViewModel.Year!.year,
                        GenderMale = _addSubscriptionViewModel.GenderMale,
                        Phone = _addSubscriptionViewModel.Phone,
                        SubscribeDate = _addSubscriptionViewModel.SubscribeDate,
                        SubscribeEndDate = _addSubscriptionViewModel.SubscribeDate,
                        IsSubscribed = true,
                    };
                    await _playerDataStore.AddPlayer(player);
                }
                Subscription subscription = new()
                {
                    DaysCount = _addSubscriptionViewModel.SubscribeDays,
                    Sport = _addSubscriptionViewModel.SelectedSport?.Sport,
                    LastCheck = _addSubscriptionViewModel.SubscribeDate,
                    Trainer = _addSubscriptionViewModel.SelectedTrainer?.trainer,
                    Player = player!,
                    RollDate = _addSubscriptionViewModel.SubscribeDate,
                    LastPaid = _addSubscriptionViewModel.SubscribeDate,
                    OfferValue = _addSubscriptionViewModel.OfferValue,
                    OfferDes = _addSubscriptionViewModel.Offer,
                    EndDate = _addSubscriptionViewModel.SubscribeDate.AddDays(_addSubscriptionViewModel.SubscribeDays),
                };
                if (_addSubscriptionViewModel.DaysCounter)
                {
                    subscription.Price = _addSubscriptionViewModel.SelectedSport!.Sport.Price;
                    subscription.PriceAfterOffer = subscription.Price - _addSubscriptionViewModel.OfferValue;
                }
                else
                {
                    subscription.Price = _addSubscriptionViewModel.SelectedSport!.Sport.DailyPrice * _addSubscriptionViewModel.SubscribeDays;
                    subscription.PriceAfterOffer = subscription.Price - _addSubscriptionViewModel.OfferValue;
                }
                DateTime payd = Convert.ToDateTime(_addSubscriptionViewModel.SubscribeDate.ToShortDateString());
                if (_addSubscriptionViewModel.PaymentValue > 0)
                {
                    PlayerPayment payment = new PlayerPayment()
                    {
                        PayDate = payd,
                        PaymentValue = _addSubscriptionViewModel.PaymentValue,
                        Des = _addSubscriptionViewModel.Descriptiones,
                        Player = player
                    };
                    subscription.Payments?.Add(payment);
                }
                await _subscriptionDataStore.Add(subscription);
                MessageBox.Show("تم الحفظ بنجاح");
                _addSubscriptionViewModel.ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
