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
        private readonly PaymentDataStore _paymentDataStore;
        public CreateMainSubscriptionCommand(SubscriptionDataStore subscriptionDataStore, CreateSubscriptionWindowViewModel addSubscriptionViewModel, PlayersDataStore playerDataStore, PaymentDataStore paymentDataStore)
        {
            _subscriptionDataStore = subscriptionDataStore;
            _addSubscriptionViewModel = addSubscriptionViewModel;
            _playerDataStore = playerDataStore;
            _paymentDataStore = paymentDataStore;
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

            return _addSubscriptionViewModel.CanSubmit &&  base.CanExecute(null);
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
                        BirthDate = _addSubscriptionViewModel.Year?.year??DateTime.Now.Year,
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
                    SportId = _addSubscriptionViewModel.SelectedSport!.Sport.Id,
                    SportSyncId = _addSubscriptionViewModel.SelectedSport!.Sport.SyncId,
                    SportName = _addSubscriptionViewModel.SelectedSport!.SportName,
                    LastCheck = _addSubscriptionViewModel.SubscribeDate,
                    TrainerId = _addSubscriptionViewModel.SelectedTrainer?.trainer.Id,
                    TrainerSyncId = _addSubscriptionViewModel.SelectedTrainer?.trainer.SyncId,
                    TrainerName = _addSubscriptionViewModel.SelectedTrainer?.TrainerName,
                    PlayerId = player!.Id,
                    PlayerSyncId = player!.SyncId,
                    PlayerName = player!.FullName,
                    RollDate = _addSubscriptionViewModel.SubscribeDate,
                    OfferValue = _addSubscriptionViewModel.OfferValue,
                    OfferDes = _addSubscriptionViewModel.Offer,
                    Price = _addSubscriptionViewModel.SelectedSport!.Price,
                    EndDate = _addSubscriptionViewModel.SubscribeDate.AddDays(_addSubscriptionViewModel.SubscribeDays),
                    PriceAfterOffer= _addSubscriptionViewModel.Total??0,
                    Code = _addSubscriptionViewModel.Code
                };
                await _subscriptionDataStore.Add(subscription);
                if (_addSubscriptionViewModel.IsRenewal)
                {
                    await _subscriptionDataStore.MarkAsRenewed(_addSubscriptionViewModel.RenewedSubscriptionId);
                }
                DateTime payd = Convert.ToDateTime(_addSubscriptionViewModel.SubscribeDate.ToShortDateString());
                if (_addSubscriptionViewModel.PaymentValue > 0)
                {
                    PlayerPayment payment = new PlayerPayment()
                    {
                        PayDate = payd,
                        PaymentValue = _addSubscriptionViewModel.PaymentValue,
                        Des = _addSubscriptionViewModel.Descriptiones,
                        PlayerId = player.Id,
                        SubscriptionId = subscription.Id
                    };
                   await _paymentDataStore.Add(payment);
                    subscription.Payments?.Add(payment);
                }
                double value = subscription.TotalPaid - subscription.PriceAfterOffer;
                _playerDataStore.UpdatePlayerBalance(player.Id, value);
                _playerDataStore.UpdatePlayerDate(player.Id, subscription.EndDate);
                
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
