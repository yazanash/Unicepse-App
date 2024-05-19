using PlatinumGym.Core.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlatinumGymPro.ViewModels.SubscriptionViewModel
{
    public class SubscriptionCardViewModel : ViewModelBase
    {
        public Subscription Subscription;
        //private readonly PlayerListViewModel playerListingViewModel;
        public int Id => Subscription.Id;
        public string? SportName => Subscription.Sport!.Name;
        public int DaysCount => Subscription.DaysCount;
        public DateTime LastCheck => Subscription.LastCheck;
        public string? Trainer => Subscription.Trainer != null ? Subscription.Trainer!.FullName : "بدون مدرب";
        public string RollDate => Subscription.RollDate.ToShortDateString();
        public double Price => Subscription.Price;
        public double OfferValue => Subscription.OfferValue;
        public string? OfferDes => Subscription.OfferDes;
        public double PriceAfterOffer => Subscription.PriceAfterOffer;
        public string IsPrivate => Subscription.IsPrivate ? "تدريب خاص" : "لا يوجد";
        public double PrivatePrice => Subscription.IsPrivate ? Subscription.PrivatePrice : 0;
        public string IsPaid => Subscription.IsPaid ? "مدفوع" : "غير مدفوع";
        public Brush IsPaidColor => Subscription.IsPaid ? Brushes.Green : Brushes.Red;
        public double PaidValue => Subscription.PaidValue;
        public string EndDate => Subscription.EndDate.ToShortDateString();
        public double RestVal => Subscription.PriceAfterOffer - Subscription.PaidValue;

        private bool _infoOpen;
        public bool InfoOpen
        {
            get { return _infoOpen; }
            set
            {
                _infoOpen = value;
                OnPropertyChanged(nameof(InfoOpen));
            }
        }
        private bool _offerOpen;
        public bool OfferOpen
        {
            get { return _offerOpen; }
            set
            {
                _offerOpen = value;
                OnPropertyChanged(nameof(OfferOpen));
            }
        }
        private bool _privateOpen;
        public bool PrivateOpen
        {
            get { return _privateOpen; }
            set
            {
                _privateOpen = value;
                OnPropertyChanged(nameof(PrivateOpen));
            }
        }

        private bool _paymentOpen;
        public bool PaymentOpen
        {
            get { return _paymentOpen; }
            set
            {
                _paymentOpen = value;
                OnPropertyChanged(nameof(PaymentOpen));
            }
        }

        public SubscriptionCardViewModel(Subscription subscription)
        {
            Subscription = subscription;
        }

    }
}
