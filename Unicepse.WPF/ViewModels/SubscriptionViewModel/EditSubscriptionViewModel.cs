using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Sport;
using Unicepse.WPF.Commands.SubscriptionCommand;
using Unicepse.WPF.Commands.Player;
using Unicepse.WPF.navigation;
using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.PaymentsViewModels;
using Unicepse.WPF.ViewModels.PlayersViewModels;
using Unicepse.WPF.ViewModels.SportsViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using emp = Unicepse.Core.Models.Employee;
using Unicepse.WPF.navigation.Stores;

namespace Unicepse.WPF.ViewModels.SubscriptionViewModel
{
    public class EditSubscriptionViewModel : ErrorNotifyViewModelBase
    {
        //private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly SportDataStore _sportDataStore;
        private readonly PlayerMainPageViewModel _playerMainPageView;
        private readonly ObservableCollection<SportListItemViewModel> _sportListItemViewModels;
        private readonly ObservableCollection<SubscriptionTrainerListItem> _trainerListItemViewModels;
        private NavigationStore _navigatorStore;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PlayersDataStore _playerDataStore;
        public IEnumerable<SportListItemViewModel> SportList => _sportListItemViewModels;
        public IEnumerable<SubscriptionTrainerListItem> TrainerList => _trainerListItemViewModels;
        public ICommand LoadSportsCommand { get; }
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
                    if (DaysCounter)
                        SubscribeDays = SelectedSport!.DaysCount;
                    else
                        SubscribeDays = 1;
                    CountTotal();
                    OnPropertyChanged(nameof(Total));
                }
                //CountTotal();
                //OnPropertyChanged(nameof(Total));

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
                OnPropertyChanged(nameof(SelectedTrainer));
            }
        }
        //private readonly PlayersDataStore _playersDataStore;

        public EditSubscriptionViewModel(SportDataStore sportDataStore, NavigationStore navigatorStore, SubscriptionDataStore subscriptionStore, PlayersDataStore playerDataStore, PlayerMainPageViewModel playerMainPageView)
        {
            _sportListItemViewModels = new ObservableCollection<SportListItemViewModel>();
            _trainerListItemViewModels = new ObservableCollection<SubscriptionTrainerListItem>();
            _navigatorStore = navigatorStore;
            _sportDataStore = sportDataStore;
            _subscriptionStore = subscriptionStore;
            _playerDataStore = playerDataStore;
            _playerMainPageView = playerMainPageView;
            LoadSportsCommand = new LoadSportItemsCommand(_sportDataStore);

            //Total = _subscriptionStore.SelectedSubscription!.Price;
            Offer = _subscriptionStore.SelectedSubscription!.OfferDes;
            OfferValue = _subscriptionStore.SelectedSubscription!.OfferValue;
            PrivatePrice = _subscriptionStore.SelectedSubscription!.PrivatePrice;
            SubscribeDate = _subscriptionStore.SelectedSubscription!.RollDate;
            DaysCounter = _subscriptionStore.SelectedSubscription.DaysCount == _subscriptionStore.SelectedSubscription.Sport!.DaysCount;
            SubscribeDays = _subscriptionStore.SelectedSubscription.DaysCount;
            PrivateProvider = _subscriptionStore.SelectedSubscription.IsPlayerPay;

            _sportDataStore.Loaded += _sportDataStore_Loaded;
            _subscriptionStore.StateChanged += _subscriptionStore_StateChanged;
            //SelectedTrainer = TrainerList.SingleOrDefault(x=>x.tr);
            SubmitCommand = new EditSubscriptionCommand(_subscriptionStore, this, _playerDataStore, new NavigationService<PlayerMainPageViewModel>(_navigatorStore, () => _playerMainPageView));
        }


        private void _subscriptionStore_StateChanged(Sport? sport)
        {
            _trainerListItemViewModels.Clear();
            foreach (var trainer in sport!.Trainers!)
            {
                AddTrainer(trainer);
            }
            if (_subscriptionStore.SelectedSubscription!.Sport!.Id == sport.Id)
                if (_subscriptionStore.SelectedSubscription!.Trainer != null)
                    SelectedTrainer = TrainerList.FirstOrDefault(x => x.Id == _subscriptionStore.SelectedSubscription!.Trainer!.Id);
        }
        private void CountTotal()
        {
            if (SelectedSport != null)
            {
                if (DaysCounter)
                    SportPrice = SelectedSport!.Price;
                else
                    SportPrice = SelectedSport!.DailyPrice * SubscribeDays;

                Total = SportPrice - OfferValue;
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
        private bool _daysCounter = true;
        public bool DaysCounter
        {
            get { return _daysCounter; }
            set
            {
                _daysCounter = value;
                CountTotal();
                if (SelectedSport != null)
                    if (DaysCounter)
                        SubscribeDays = SelectedSport!.DaysCount;
                    else
                        SubscribeDays = 1;
                OnPropertyChanged(nameof(DaysCounter));
                OnPropertyChanged(nameof(Total));

            }
        }

        //private double? _total;
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
        private bool _privateProvider;
        public bool PrivateProvider
        {
            get { return _privateProvider; }
            set
            {
                _privateProvider = value;
                OnPropertyChanged(nameof(PrivateProvider));

            }
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

            SelectedSport = SportList.FirstOrDefault(x => x.Sport.Id == _subscriptionStore.SelectedSubscription!.Sport!.Id);
        }
        private void AddSport(Sport sport)
        {
            SportListItemViewModel itemViewModel =
                new SportListItemViewModel(sport, _sportDataStore, _navigatorStore);
            _sportListItemViewModels.Add(itemViewModel);
        }

        private void AddTrainer(emp.Employee trainer)
        {
            SubscriptionTrainerListItem itemViewModel =
                new SubscriptionTrainerListItem(trainer);
            _trainerListItemViewModels.Add(itemViewModel);
        }
        public static EditSubscriptionViewModel LoadViewModel(SportDataStore sportDataStore, NavigationStore navigatorStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, PlayerMainPageViewModel playerMainPageViewModel)
        {
            EditSubscriptionViewModel viewModel = new(sportDataStore, navigatorStore, subscriptionDataStore, playersDataStore, playerMainPageViewModel);

            viewModel.LoadSportsCommand.Execute(null);

            return viewModel;
        }
        public override void Dispose()
        {
            _subscriptionStore.StateChanged -= _subscriptionStore_StateChanged;
            _subscriptionStore.Loaded -= _sportDataStore_Loaded;
            base.Dispose();
        }
    }
}
