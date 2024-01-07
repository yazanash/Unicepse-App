using PlatinumGym.Core.Models.Subscription;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlatinumGymPro.ViewModels.SubscriptionViewModel
{
    public class SubscriptionListItemViewModel : ViewModelBase
    {
        public Subscription Subscription;
        private readonly NavigationStore _navigationStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        //private readonly PlayerListViewModel playerListingViewModel;
        public string? SportName => Subscription.Sport!.Name;
        public DateTime LastCheck => Subscription.LastCheck;
        public string? Trainer => Subscription.Trainer!.FullName;
        public string RollDate => Subscription.RollDate.ToString("ddd,MMM dd,yyy");
        public double Price => Subscription.Price;
        public double OfferValue => Subscription.OfferValue;
        public string? OfferDes => Subscription.OfferDes;
        public double PriceAfterOffer => Subscription.PriceAfterOffer;
        public string IsPrivate => Subscription.IsPrivate ? "تدريب خاص" : "لا يوجد";
        //public bool IsStopped => Subscription.IsStopped? "اشتراك مسحوب" : "لا يوجد";
        //public bool IsMoved { get; set; }
        public double PrivatePrice => Subscription.PrivatePrice;
        public string IsPaid => Subscription.IsPaid ? "مدفوع" : "غير مدفوع";
        public Brush IsPaidColor => Subscription.IsPaid ? Brushes.Green : Brushes.Red;
        public double PaidValue => Subscription.PaidValue;
        //public double RestValue { get; set; }
        public string EndDate => Subscription.EndDate.ToString("ddd,MMM dd,yyy");
        //public DateTime LastPaid => Subscription


        //public ICommand? EditCommand { get; }
        //public ICommand? DeleteCommand { get; }
        //public ICommand? OpenProfileCommand { get; }
        public SubscriptionListItemViewModel(Subscription subscription, NavigationStore navigationStore, SubscriptionDataStore subscriptionDataStore)
        {
            Subscription = subscription;

            _subscriptionDataStore = subscriptionDataStore;
            _navigationStore = navigationStore;
            //this.playerListingViewModel = playerListingViewModel;
            //EditCommand = new NavaigateCommand<EditPlayerViewModel>(new NavigationService<EditPlayerViewModel>(_navigationStore, () => new EditPlayerViewModel(_navigationStore, player)));
            //OpenProfileCommand = new NavaigateCommand<PlayerProfileViewModel>(new NavigationService<PlayerProfileViewModel>(_navigationStore, () => new PlayerProfileViewModel(_navigationStore, subscriptionDataStore, player)));
        }

        public void Update(Subscription subscription)
        {
            this.Subscription = subscription;

            OnPropertyChanged(nameof(SportName));
        }
    }
}
