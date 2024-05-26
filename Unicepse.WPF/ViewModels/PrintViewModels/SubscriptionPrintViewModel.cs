using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Subscription;
using Unicepse.WPF.ViewModels.PaymentsViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.WPF.utlis.common;

namespace Unicepse.WPF.ViewModels.PrintViewModels
{
    public class SubscriptionPrintViewModel : ViewModelBase
    {
        public Subscription Subscription;
        private readonly ObservableCollection<PaymentListItemViewModel> _paymentListItemViewModels;

        public IEnumerable<PaymentListItemViewModel> PaymentsList => _paymentListItemViewModels;
        public SubscriptionPrintViewModel(Subscription subscription)
        {
            Subscription = subscription;
            _paymentListItemViewModels = new ObservableCollection<PaymentListItemViewModel>();
            foreach(var pay in Subscription.Payments!)
            {
                PaymentListItemViewModel paymentListItemViewModel = new PaymentListItemViewModel(pay);
                _paymentListItemViewModels.Add(paymentListItemViewModel);
            }
        }

        public int Id => Subscription.Id;
        public string? PlayerName => Subscription.Player!.FullName;
        public double PlayerBalance => Subscription.Player!.Balance;
        public string? SportName => Subscription.Sport!.Name;
        public int SubDays => Subscription.DaysCount;
        public DateTime LastCheck => Subscription.LastCheck;
        public string? Trainer => Subscription.Trainer != null ? Subscription.Trainer!.FullName : "بدون مدرب";
        public string RollDate => Subscription.RollDate.ToString("ddd,MMM dd,yyy");
        public double Price => Subscription.Price;
        public double OfferValue => Subscription.OfferValue;
        public string? OfferDes => Subscription.OfferDes;
        public double PriceAfterOffer => Subscription.PriceAfterOffer;
        public string IsPrivate => Subscription.IsPrivate ? "تدريب خاص" : "لا يوجد";
        public double PrivatePrice => Subscription.IsPrivate ? Subscription.PrivatePrice : 0;
        public double PaidValue => Subscription.PaidValue;
        public string EndDate => Subscription.EndDate.ToString("ddd,MMM dd,yyy");
    }
}
