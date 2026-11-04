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
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Subscription;
using Uniceps.navigation;
using Uniceps.navigation.NavigationSystem;
using Uniceps.Stores;
using Uniceps.SystemServices;
using Uniceps.utlis.common;
using Uniceps.ViewModels.PlayersViewModels;

namespace Uniceps.ViewModels.SubscriptionViewModel
{
   
    public class SubscriptionListItemViewModel : ViewModelBase
    {
        public Subscription Subscription;
        private readonly SubscriptionDataStore? _subscriptionDataStore;
        public int Id => Subscription.Id;
        private int _order;
        public int Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged(nameof(Order)); }
        }
        public string? PlayerName => Subscription.Player?.FullName;
        public string? SportName => Subscription.SportName?? $"رياضة محذوفة";
        public string? PlayerPhone => Subscription.Player?.Phone;
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
        public DateTime EndDateFull => Subscription.EndDate;
        public DateTime RollDateFull => Subscription.RollDate;
        public string EndDate => Subscription.EndDate.ToShortDateString();
        public double RestDays
        {
            get
            {
                // 1. أخذ تاريخ انتهاء الاشتراك وتاريخ اليوم بدون ساعات ولا دقائق (.Date)
                DateTime endDate = Subscription.EndDate.Date;
                DateTime today = DateTime.Now.Date;

                // 2. حساب الفرق بالأيام الحقيقية (Calendar Days)
                int remainingDays = (endDate - today).Days;

                // 3. إرجاع عدد الأيام المتبقية (شاملة يوم الانتهاء نفسه)
                // إذا كان اليوم 31/07 والانتهاء 01/08 فالنتيجة = 1 + 1 = 2 أيام (اليوم وبكرا)
                return remainingDays >= 0 ? remainingDays + 1 : 0;
            }
        }
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
        public SubscriptionListItemViewModel(Subscription subscription, SubscriptionDataStore subscriptionDataStore)
        {
            Subscription = subscription;
            _subscriptionDataStore = subscriptionDataStore;
            DeleteCommand = new DeleteSubscriptionCommand(_subscriptionDataStore);
            //StopSubscriptionCommand = new RelayCommand(()=>_navigationService.Navigate<StopSubscriptionViewModel>());
            CheckSubscriptionStatus();
        }
        private void CheckSubscriptionStatus()
        {
            int reminderDays = SettingsManager.Current.SubscriptionRemainderDays;

            DateTime endDate = Subscription.EndDate.Date;
            DateTime today = DateTime.Now.Date;

            int calendarDaysLeft = (endDate - today).Days;

            if (Subscription.IsRenewed)
            {
                SubscriptionStatus = SubscriptionStatus.Renewed;
            }
            else if (calendarDaysLeft < 0)
            {
                SubscriptionStatus = SubscriptionStatus.Expired;
            }
            else if (calendarDaysLeft <= reminderDays)
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
                    SubscriptionStatusString = calendarDaysLeft == 0
                        ? "ينتهي اليوم"
                        : $"سينتهي خلال {calendarDaysLeft} يوم";
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
            OnPropertyChanged(nameof(Subscription));
            OnPropertyChanged(nameof(SportName));
            OnPropertyChanged(nameof(PlayerName));
            OnPropertyChanged(nameof(PlayerPhone));
            OnPropertyChanged(nameof(PaidValue));
            OnPropertyChanged(nameof(RestValue));
            CheckSubscriptionStatus();
           
        }

    }
}
