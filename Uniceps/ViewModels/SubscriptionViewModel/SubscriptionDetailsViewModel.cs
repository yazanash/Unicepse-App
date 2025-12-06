using Uniceps.Commands.SubscriptionCommand;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.ViewModels;
using Uniceps.Stores;
using Uniceps.navigation.Stores;
using Uniceps.ViewModels.SportsViewModels;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.PaymentsViewModels;
using Uniceps.Core.Models.Sport;

namespace Uniceps.ViewModels.SubscriptionViewModel
{
    public class SubscriptionDetailsViewModel : ErrorNotifyViewModelBase
    {
        private readonly SportDataStore _sportDataStore;
        private readonly ObservableCollection<SportListItemViewModel> _sportListItemViewModels;
        private readonly ObservableCollection<SubscriptionTrainerListItem> _trainerListItemViewModels;
        private NavigationStore _navigatorStore;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PlayersDataStore _playerDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly PlayerMainPageViewModel _playerMainPageView;

        public SportListItemViewModel? SelectedSport
        {
            get
            {
                return SportList
                    .FirstOrDefault(y => y?.Sport == _subscriptionStore.SelectedSport);
            }
            set
            {
                _subscriptionStore.SelectedSport = value?.Sport;

                OnPropertyChanged(nameof(SelectedSport));
                ClearError(nameof(SelectedSport));
                if (SelectedSport == null)
                {
                    AddError(nameof(SelectedSport), "");
                    OnErrorChanged(nameof(SelectedSport));
                }
                else
                {
                  
                        SubscribeDays = SelectedSport!.DaysCount;
                    SportPrice = SelectedSport!.Price;
                    CountTotal();
                    OnPropertyChanged(nameof(Total));
                }

            }
        }

        public SubscriptionTrainerListItem? SelectedTrainer
        {
            get
            {
                return TrainerList
                    .FirstOrDefault(y => y?.trainer == _subscriptionStore.SelectedTrainer);
            }
            set
            {
                _subscriptionStore.SelectedTrainer = value?.trainer;

            }
        }
        public IEnumerable<SportListItemViewModel> SportList => _sportListItemViewModels;
        public IEnumerable<SubscriptionTrainerListItem> TrainerList => _trainerListItemViewModels;
        public ICommand LoadSportsCommand { get; }
        public SubscriptionDetailsViewModel(SportDataStore sportDataStore, NavigationStore navigatorStore, SubscriptionDataStore subscriptionStore, PlayersDataStore playerDataStore, PaymentDataStore paymentDataStore, PlayerMainPageViewModel playerMainPageView)
        {
            _navigatorStore = navigatorStore;
            _sportDataStore = sportDataStore;
            _subscriptionStore = subscriptionStore;
            _playerDataStore = playerDataStore;
            _paymentDataStore = paymentDataStore;
            _playerMainPageView = playerMainPageView;

            CancelCommand = new NavaigateCommand<PlayerMainPageViewModel>(new NavigationService<PlayerMainPageViewModel>(_navigatorStore, () => _playerMainPageView));
            _sportListItemViewModels = new ObservableCollection<SportListItemViewModel>();
            _trainerListItemViewModels = new ObservableCollection<SubscriptionTrainerListItem>();
            _sportDataStore.Loaded += _sportDataStore_Loaded;
            _subscriptionStore.StateChanged += _subscriptionStore_StateChanged;
            LoadSportsCommand = new LoadSportItemsCommand(_sportDataStore);
            SubmitCommand = new CreateSubscriptionCommand(_subscriptionStore, this, _playerDataStore, new NavigationService<AddPaymentViewModel>(_navigatorStore, () => CreatePaymentViewModel(_paymentDataStore, _subscriptionStore, _playerDataStore, _navigatorStore, CreatePaymentListViewModel(_paymentDataStore, _playerDataStore, _navigatorStore, _subscriptionStore))));
        }

        private static AddPaymentViewModel CreatePaymentViewModel(PaymentDataStore paymentDataStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, NavigationStore navigationStore, PaymentListViewModel paymentListViewModel)
        {
            return AddPaymentViewModel.LoadViewModel(paymentDataStore, subscriptionDataStore, playersDataStore, navigationStore, paymentListViewModel);
        }
        private static PaymentListViewModel CreatePaymentListViewModel(PaymentDataStore paymentDataStore, PlayersDataStore playersDataStore, NavigationStore navigationStore, SubscriptionDataStore subscriptionDataStore)
        {
            return new PaymentListViewModel(paymentDataStore, playersDataStore, navigationStore, subscriptionDataStore);
        }
        private void _subscriptionStore_StateChanged(Sport? sport)
        {
            _trainerListItemViewModels.Clear();
            if (_subscriptionStore.SelectedSport != null)
                foreach (var trainer in sport!.Trainers!)
                {
                    AddTrainer(trainer);
                }
        }

        private void CountTotal()
        {
            if (SelectedSport != null)
            {
                
                SportPrice = SelectedSport!.Price;
                Total = SportPrice - OfferValue;
                ClearError(nameof(Total));
                if (Total < 0)
                {
                    AddError("لا يمكن ان يكون الاشتراك اقل من 0", nameof(Total));
                    OnErrorChanged(nameof(Total));
                }
            }

        }

        #region Properties
        private double SportPrice { get; set; }
        private int _subscribeDays;
        public int SubscribeDays
        {
            get { return _subscribeDays; }
            set
            {
                _subscribeDays = value;
                OnPropertyChanged(nameof(SubscribeDays));
                CountTotal();
                OnPropertyChanged(nameof(Total));
            }
        }

        public double? Total
        {
            get;
            set;
        }

        private string? _offer;
        public string? Offer
        {
            get { return _offer; }
            set { _offer = value; OnPropertyChanged(nameof(Offer)); }
        }
        private double _offerValue;
        public double OfferValue
        {
            get { return _offerValue; }
            set
            {
                _offerValue = value;
                CountTotal();
                OnPropertyChanged(nameof(OfferValue));
                OnPropertyChanged(nameof(Total));
            }
        }
        private DateTime _subscribeDate = DateTime.Now.Date;
        public DateTime SubscribeDate
        {
            get { return _subscribeDate; }
            set { _subscribeDate = value; OnPropertyChanged(nameof(SubscribeDate)); }
        }
        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
        #endregion
        private void _sportDataStore_Loaded()
        {
            _sportListItemViewModels.Clear();

            foreach (Sport sport in _sportDataStore.Sports)
            {
                AddSport(sport);
            }
            SelectedSport = _sportListItemViewModels.FirstOrDefault();

        }
        private void AddSport(Sport sport)
        {
            SportListItemViewModel itemViewModel =
                new SportListItemViewModel(sport, _sportDataStore, _navigatorStore);
            _sportListItemViewModels.Add(itemViewModel);
        }

        public override void Dispose()
        {
            _sportDataStore.Loaded -= _sportDataStore_Loaded;
            _subscriptionStore.StateChanged -= _subscriptionStore_StateChanged;
            base.Dispose();
        }
        private void AddTrainer(Core.Models.Employee.Employee trainer)
        {
            SubscriptionTrainerListItem itemViewModel =
                new SubscriptionTrainerListItem(trainer);
            _trainerListItemViewModels.Add(itemViewModel);
        }
        public static SubscriptionDetailsViewModel LoadViewModel(SportDataStore sportDataStore, NavigationStore navigatorStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, PaymentDataStore paymentDataStore, PlayerMainPageViewModel playerMainPageViewModel)
        {
            SubscriptionDetailsViewModel viewModel = new(sportDataStore, navigatorStore, subscriptionDataStore, playersDataStore, paymentDataStore, playerMainPageViewModel);

            viewModel.LoadSportsCommand.Execute(null);

            return viewModel;
        }
    }
}
