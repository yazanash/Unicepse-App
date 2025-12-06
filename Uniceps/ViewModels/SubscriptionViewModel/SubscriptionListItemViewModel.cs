using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Commands.PlayerAttendenceCommands;
using Uniceps.Commands.SubscriptionCommand;
using Uniceps.Core.Models.Subscription;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.PrintViewModels;

namespace Uniceps.ViewModels.SubscriptionViewModel
{
   
    public class SubscriptionListItemViewModel : ViewModelBase
    {
        public Subscription Subscription;
        private readonly NavigationStore? _navigationStore;
        private readonly SubscriptionDataStore? _subscriptionDataStore;
        private readonly SportDataStore? _sportDataStore;
        private readonly PlayersDataStore? _playersDataStore;
        private readonly PaymentDataStore? _paymentDataStore;
        private readonly PlayerMainPageViewModel? _playerMainPageViewModel;
        public int Id => Subscription.Id;
        private int _order;
        public int Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged(nameof(Order)); }
        }
        public string? PlayerName => Subscription.PlayerName;
        public string? SportName => Subscription.SportName;
        public DateTime LastCheck => Subscription.LastCheck;
        public string? Trainer => Subscription.TrainerName ?? "بدون مدرب";
        public string RollDate => Subscription.RollDate.ToShortDateString();
        public double Price => Subscription.Price;
        public string? Code => Subscription.Code;
        public double OfferValue => Subscription.OfferValue;
        public string? OfferDes => Subscription.OfferDes;
        public double PriceAfterOffer => Subscription.PriceAfterOffer;
        public double PaidValue => Subscription.TotalPaid;
        public double RestValue => Subscription.Remaining;
        public bool IsRenewed => Subscription.IsRenewed;
        //public double RestValue { get; set; }
        public string EndDate => Subscription.EndDate.ToShortDateString();
        //public DateTime LastPaid => Subscription

        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            set { _isOpen = value; OnPropertyChanged(nameof(IsOpen)); }

        }
        private bool _isLoggedIn = false;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; OnPropertyChanged(nameof(IsLoggedIn));
                OnPropertyChanged(nameof(IsLoggedBrush));
                OnPropertyChanged(nameof(IsLoggedString));
                OnPropertyChanged(nameof(IsLoggedIcon));
            }

        }
        public Brush IsLoggedBrush => IsLoggedIn ? Brushes.Red : Brushes.Green;
        public string? IsLoggedString=> IsLoggedIn ? "تسجيل خروج" : "تسجيل دخول";
        public string? IsLoggedIcon => IsLoggedIn ? "Logout" : "Login";
        public ICommand? EditCommand { get; }
        public ICommand? DeleteCommand { get; }
        public ICommand? StopSubscriptionCommand { get; }
        public SubscriptionStatus SubscriptionStatus { get; set; }
        public string? SubscriptionStatusString { get; set; }
        public Brush? SubscriptionStatusColor { get; set; }
        public string? SubscriptionStatusIcon { get; set; }
        public SubscriptionListItemViewModel(Subscription subscription, NavigationStore navigationStore, SubscriptionDataStore subscriptionDataStore, SportDataStore sportDataStore, PlayersDataStore playersDataStore, PlayerMainPageViewModel playerMainPageViewModel, PaymentDataStore paymentDataStore)
        {
            Subscription = subscription;
            _sportDataStore = sportDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _navigationStore = navigationStore;
            _paymentDataStore = paymentDataStore;
            _playersDataStore = playersDataStore;
            _playerMainPageViewModel = playerMainPageViewModel;
            DeleteCommand = new DeleteSubscriptionCommand(_subscriptionDataStore);
            EditCommand = new NavaigateCommand<EditSubscriptionViewModel>(new NavigationService<EditSubscriptionViewModel>(_navigationStore, () => EditSubscription(_sportDataStore, _navigationStore, _subscriptionDataStore, _playersDataStore, _playerMainPageViewModel)));
            StopSubscriptionCommand = new NavaigateCommand<StopSubscriptionViewModel>(new NavigationService<StopSubscriptionViewModel>(_navigationStore, () => new StopSubscriptionViewModel(_navigationStore, _subscriptionDataStore, _playersDataStore, _paymentDataStore, _playerMainPageViewModel)));
            CheckSubscriptionStatus();
        }
        private void CheckSubscriptionStatus()
        {
            if (Subscription.IsRenewed)
            {
                SubscriptionStatus = SubscriptionStatus.Renewed;
            }
            else if (Subscription.EndDate < DateTime.Now)
            {
                SubscriptionStatus = SubscriptionStatus.Expired;
            }
            else if (Subscription.EndDate.Subtract(DateTime.Now).TotalDays <= 2)
            {
                SubscriptionStatus = SubscriptionStatus.EndingSoon;
            }
            else
            {
                SubscriptionStatus = SubscriptionStatus.Active;
            }
            switch (SubscriptionStatus)
            {
                case SubscriptionStatus.EndingSoon:
                    SubscriptionStatusString = "سينتهي خلال يومين";
                    SubscriptionStatusColor = Brushes.Yellow;
                    SubscriptionStatusIcon = "AlertOutline";
                    break;
                case SubscriptionStatus.Expired:
                    SubscriptionStatusString = "الاشتراك منتهي";
                    SubscriptionStatusColor = Brushes.Red;
                    SubscriptionStatusIcon = "AlertOctagonOutline";
                    break;
                case SubscriptionStatus.Renewed:
                    SubscriptionStatusString = "الاشتراك مجدد";
                    SubscriptionStatusColor = Brushes.Green;
                    SubscriptionStatusIcon = "CheckboxMarkedCircleOutline";
                    break;
                case SubscriptionStatus.Active:
                    SubscriptionStatusString = "فعال";
                    SubscriptionStatusColor = Brushes.LightBlue;
                    SubscriptionStatusIcon = "CheckDecagramOutline";
                    break;
            }
            OnPropertyChanged(nameof(SubscriptionStatusString));
            OnPropertyChanged(nameof(SubscriptionStatusColor));
            OnPropertyChanged(nameof(SubscriptionStatusIcon));
        }
        public SubscriptionListItemViewModel(Subscription subscription)
        {
            Subscription = subscription;
            CheckSubscriptionStatus();
        }
        public ICommand? LogInCommand { get; }


        public void Update(Subscription subscription)
        {
            Subscription = subscription;
            OnPropertyChanged(nameof(SportName));

            CheckSubscriptionStatus();
           
        }
        public static EditSubscriptionViewModel EditSubscription(SportDataStore sportDataStore, NavigationStore navigationStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, PlayerMainPageViewModel playerMainPageViewModel)
        {
            return EditSubscriptionViewModel.LoadViewModel(sportDataStore, navigationStore, subscriptionDataStore, playersDataStore, playerMainPageViewModel);

        }

    }
}
