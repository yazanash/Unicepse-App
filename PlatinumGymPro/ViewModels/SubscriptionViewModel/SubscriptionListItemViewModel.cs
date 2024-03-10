using PlatinumGym.Core.Models.Subscription;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace PlatinumGymPro.ViewModels.SubscriptionViewModel
{
    public class SubscriptionListItemViewModel : ViewModelBase
    {
        public Subscription Subscription;
        private readonly NavigationStore _navigationStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly SportDataStore _sportDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly PlayerMainPageViewModel _playerMainPageViewModel;
        //private readonly PlayerListViewModel playerListingViewModel;
        public int Id => Subscription.Id;
        public string? SportName => Subscription.Sport!.Name;
        public DateTime LastCheck => Subscription.LastCheck;
        public string? Trainer =>  Subscription.Trainer != null ? Subscription.Trainer!.FullName:"بدون مدرب";
        public string RollDate => Subscription.RollDate.ToString("ddd,MMM dd,yyy");
        public double Price => Subscription.Price;
        public double OfferValue => Subscription.OfferValue;
        public string? OfferDes => Subscription.OfferDes;
        public double PriceAfterOffer => Subscription.PriceAfterOffer;
        public string IsPrivate => Subscription.IsPrivate ? "تدريب خاص" : "لا يوجد";
        //public bool IsStopped => Subscription.IsStopped? "اشتراك مسحوب" : "لا يوجد";
        //public bool IsMoved { get; set; }
        public double PrivatePrice => Subscription.IsPrivate ? Subscription.PrivatePrice : 0;
        public string IsPaid => Subscription.IsPaid ? "مدفوع" : "غير مدفوع";
        public Brush IsPaidColor => Subscription.IsPaid ? Brushes.Green : Brushes.Red;
        //public Brush IsPaidTextColor => !Subscription.IsPaid ? new BrushConverter().ConvertFromString("#80A894") as SolidColorBrush : new BrushConverter().ConvertFromString("#BE99C3") as SolidColorBrush;
        public double PaidValue => Subscription.PaidValue;
        //public double RestValue { get; set; }
        public string EndDate => Subscription.EndDate.ToString("ddd,MMM dd,yyy");
        //public DateTime LastPaid => Subscription


        public ICommand? EditCommand { get; }
        //public ICommand? DeleteCommand { get; }
        public ICommand? StopSubscriptionCommand { get; }
        public SubscriptionListItemViewModel(Subscription subscription, NavigationStore navigationStore, SubscriptionDataStore subscriptionDataStore, SportDataStore sportDataStore, PlayersDataStore playersDataStore, PlayerMainPageViewModel playerMainPageViewModel, PaymentDataStore paymentDataStore)
        {
            Subscription = subscription;
            _sportDataStore = sportDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _navigationStore = navigationStore;
            _paymentDataStore = paymentDataStore;
            _playersDataStore = playersDataStore;
            _playerMainPageViewModel = playerMainPageViewModel;
            EditCommand = new NavaigateCommand<EditSubscriptionViewModel>(new NavigationService<EditSubscriptionViewModel>(_navigationStore, () => EditSubscription(_sportDataStore, _navigationStore, _subscriptionDataStore, _playersDataStore, _playerMainPageViewModel)));
            //StopSubscriptionCommand = new NavaigateCommand<StopSubscriptionViewModel>(new NavigationService<StopSubscriptionViewModel>(_navigationStore, () => new StopSubscriptionViewModel(_navigationStore, _subscriptionDataStore, _playersDataStore,_paymentDataStore,_playerMainPageViewModel)));
            StopSubscriptionCommand = new NavaigateCommand<MoveToNewTrainerViewModel>(new NavigationService<MoveToNewTrainerViewModel>(_navigationStore, () => new MoveToNewTrainerViewModel(_navigationStore, _subscriptionDataStore, _playerMainPageViewModel)));
        }

        public void Update(Subscription subscription)
        {
            this.Subscription = subscription;

            OnPropertyChanged(nameof(SportName));
        }
        public static EditSubscriptionViewModel EditSubscription(SportDataStore sportDataStore,NavigationStore navigationStore, SubscriptionDataStore subscriptionDataStore,PlayersDataStore playersDataStore , PlayerMainPageViewModel playerMainPageViewModel)
        {
            return EditSubscriptionViewModel.LoadViewModel(sportDataStore, navigationStore, subscriptionDataStore,playersDataStore, playerMainPageViewModel);

        }
      
    }
}
