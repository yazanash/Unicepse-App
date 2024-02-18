using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Models.Sport;
using PlatinumGymPro.Commands.SubscriptionCommand;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.PaymentsViewModels;
using PlatinumGymPro.ViewModels.SportsViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.SubscriptionViewModel
{
    public class SubscriptionDetailsViewModel : ViewModelBase
    {
        //private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly SportDataStore _sportDataStore;
        //private readonly EmployeeStore _employeeStore;
        private readonly ObservableCollection<SportListItemViewModel> _sportListItemViewModels;
        private readonly ObservableCollection<SubscriptionTrainerListItem> _trainerListItemViewModels;
        private NavigationStore _navigatorStore;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PlayersDataStore _playerDataStore ;
        private readonly PaymentDataStore _paymentDataStore;

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
        //private readonly PlayersDataStore _playersDataStore;
        public IEnumerable<SportListItemViewModel> SportList => _sportListItemViewModels;
        public IEnumerable<SubscriptionTrainerListItem> TrainerList => _trainerListItemViewModels;
        public ICommand LoadSportsCommand { get; }
        public SubscriptionDetailsViewModel(SportDataStore sportDataStore, NavigationStore navigatorStore, SubscriptionDataStore subscriptionStore, PlayersDataStore playerDataStore, PaymentDataStore paymentDataStore)
        {
            _navigatorStore = navigatorStore;
            _sportDataStore = sportDataStore;
            _subscriptionStore = subscriptionStore;
            _playerDataStore = playerDataStore;
            _paymentDataStore = paymentDataStore;

            _sportListItemViewModels = new ObservableCollection<SportListItemViewModel>();
            _trainerListItemViewModels = new ObservableCollection<SubscriptionTrainerListItem>();
            _sportDataStore.Loaded += _sportDataStore_Loaded;
            _subscriptionStore.StateChanged += _subscriptionStore_StateChanged;
            LoadSportsCommand = new LoadSportItemsCommand(_sportDataStore);
            SubmitCommand = new CreateSubscriptionCommand(_subscriptionStore, this, _playerDataStore, new NavigationService<AddPaymentViewModel>(_navigatorStore, () => CreatePaymentViewModel(_paymentDataStore,_subscriptionStore, _playerDataStore,_navigatorStore)));
        }

        private static AddPaymentViewModel CreatePaymentViewModel(PaymentDataStore paymentDataStore,SubscriptionDataStore subscriptionDataStore,PlayersDataStore playersDataStore,NavigationStore navigationStore)
        {
            return new AddPaymentViewModel(paymentDataStore,subscriptionDataStore, playersDataStore, navigationStore);
        }

        private void _subscriptionStore_StateChanged(Sport? sport)
        {
            foreach(var trainer in sport!.Trainers!)
            {
                AddTrainer(trainer);
            }
        }


        #region Properties

        private int _subscribeDays;
        public int SubscribeDays
        {
            get { return _subscribeDays; }
            set
            {
                _subscribeDays = value;
                OnPropertyChanged(nameof(SubscribeDays));
               
            }
        }
        private string? _total;
        public string? Total
        {
            get { return _total; }
            set
            {
                _total = value; 
                OnPropertyChanged(nameof(Total));
            }
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
            set { _offerValue = value; OnPropertyChanged(nameof(OfferValue)); }
        }
        private double _privatePrice;
        public double PrivatePrice
        {
            get { return _privatePrice; }
            set
            {
                _privatePrice = value; OnPropertyChanged(nameof(PrivatePrice));
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
        }
        private void AddSport(Sport sport)
        {
            SportListItemViewModel itemViewModel =
                new SportListItemViewModel(sport,_sportDataStore, _navigatorStore);
            _sportListItemViewModels.Add(itemViewModel);
        }

        private void AddTrainer(Employee trainer)
        {
            SubscriptionTrainerListItem itemViewModel =
                new SubscriptionTrainerListItem(trainer);
            _trainerListItemViewModels.Add(itemViewModel);
        }
        public static SubscriptionDetailsViewModel LoadViewModel(SportDataStore sportDataStore, NavigationStore navigatorStore,SubscriptionDataStore subscriptionDataStore,PlayersDataStore playersDataStore,PaymentDataStore paymentDataStore)
        {
            SubscriptionDetailsViewModel viewModel = new(sportDataStore, navigatorStore,subscriptionDataStore, playersDataStore, paymentDataStore);

            viewModel.LoadSportsCommand.Execute(null);

            return viewModel;
        }
    }
}
