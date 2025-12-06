using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.ViewModels.SubscriptionViewModel
{
    public class SubscriptionCardViewModel : ViewModelBase
    {
        public Subscription Subscription;
        //private readonly PlayerListViewModel playerListingViewModel;
        public int Id => Subscription.Id;
        public string? SportName => Subscription.SportName;
        public int DaysCount => Subscription.DaysCount;
        public DateTime LastCheck => Subscription.LastCheck;
        public string? Trainer => Subscription.TrainerName ?? "بدون مدرب";
        public string RollDate => Subscription.RollDate.ToShortDateString();
        public double Price => Subscription.Price;
        public double OfferValue => Subscription.OfferValue;
        public string? OfferDes => Subscription.OfferDes;
        public double PriceAfterOffer => Subscription.PriceAfterOffer;
        public double PaidValue => Subscription.TotalPaid;
        public string EndDate => Subscription.EndDate.ToShortDateString();

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
