using Unicepse.Core.Models.Subscription;
using Unicepse.WPF.Commands;
using Unicepse.WPF.Commands.Player;
using Unicepse.WPF.navigation;
using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.PlayersViewModels;
using Unicepse.WPF.ViewModels.PrintViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Unicepse.WPF.navigation.Stores;
using Unicepse.WPF.utlis.common;

namespace Unicepse.WPF.ViewModels.SubscriptionViewModel
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
        public string? PlayerName => Subscription.Player!.FullName;
        public string? SportName => Subscription.Sport!.Name;
        public DateTime LastCheck => Subscription.LastCheck;
        public string? Trainer =>  Subscription.Trainer != null ? Subscription.Trainer!.FullName:"بدون مدرب";
        public string RollDate => Subscription.RollDate.ToShortDateString();
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

        public double RestVal => Subscription.PriceAfterOffer - Subscription.PaidValue;
        //public double RestValue { get; set; }
        public string EndDate => Subscription.EndDate.ToShortDateString();
        //public DateTime LastPaid => Subscription

        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            set { _isOpen = value; OnPropertyChanged(nameof(IsOpen)); }

        }
        public ICommand? EditCommand { get; }
        public ICommand? PrintCommand { get; }
        public ICommand? StopSubscriptionCommand { get; }
        public ICommand? MoveToNewTrainerCommand { get; }
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
            StopSubscriptionCommand = new NavaigateCommand<StopSubscriptionViewModel>(new NavigationService<StopSubscriptionViewModel>(_navigationStore, () => new StopSubscriptionViewModel(_navigationStore, _subscriptionDataStore, _playersDataStore, _paymentDataStore, _playerMainPageViewModel)));
            MoveToNewTrainerCommand = new NavaigateCommand<MoveToNewTrainerViewModel>(new NavigationService<MoveToNewTrainerViewModel>(_navigationStore, () => new MoveToNewTrainerViewModel(_navigationStore, _subscriptionDataStore, _playerMainPageViewModel)));
            string filename = _playersDataStore.SelectedPlayer!.FullName + "_" + RollDate+ "_"+SportName;
            PrintCommand = new PrintCommand(new PrintWindowViewModel(new SubscriptionPrintViewModel(this.Subscription), new NavigationStore()), filename);
        }
        public SubscriptionListItemViewModel(Subscription subscription)
        {
            Subscription = subscription;
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
