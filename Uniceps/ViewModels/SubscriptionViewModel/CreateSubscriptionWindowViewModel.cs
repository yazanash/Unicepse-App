using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Commands.SubscriptionCommand;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;
using Uniceps.navigation.DialogService;
using Uniceps.navigation.Extensions;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.ViewModels.Employee.TrainersViewModels;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.SportsViewModels;
using Uniceps.Views.EmployeeViews;
using Uniceps.Views.PlayerViews;
using Uniceps.Views.SportViews;

namespace Uniceps.ViewModels.SubscriptionViewModel
{
    public class CreateSubscriptionWindowViewModel : ErrorNotifyViewModelBase,IQueryAttributable
    {
        private readonly IDialogService _dialogService;
        private readonly SportDataStore _sportDataStore;
        private readonly ObservableCollection<SportListItemViewModel> _sportListItemViewModels;
        private readonly ObservableCollection<SubscriptionTrainerListItem> _trainerListItemViewModels;
        private readonly ObservableCollection<PlayerListItemViewModel> _playerListItemViewModels;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PlayersDataStore _playerDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly EmployeeStore _employeeStore;

        public Subscription? ExistedSubscription;

        private SportListItemViewModel? _selectedSport;
        public SportListItemViewModel? SelectedSport
        {
            get
            {
                return _selectedSport;
            }
            set
            {
                _selectedSport = value;
                ClearError(nameof(SelectedSport));
                OnPropertyChanged(nameof(SelectedSport));
                if (SelectedSport == null)
                {
                    AddError("Sport Not Selected", nameof(SelectedSport));
                    OnErrorChanged(nameof(SelectedSport));
                }
                else
                {
                    SubscribeDays = SelectedSport!.DaysCount;
                    SportPrice = SelectedSport!.Price;
                    CountTotal();
                    OnPropertyChanged(nameof(Total));
                }
                OnPropertyChanged(nameof(CanSubmit));

            }
        }
        public bool IsClearAble { get; set; } = false;
        private SubscriptionTrainerListItem? _selectedTrainer;
        public SubscriptionTrainerListItem? SelectedTrainer
        {
            get
            {
                return _selectedTrainer;
            }
            set
            {
                _selectedTrainer = value;
                IsClearAble = SelectedTrainer != null;
                OnPropertyChanged(nameof(IsClearAble));
                OnPropertyChanged(nameof(SelectedTrainer));
            }
        }

        private PlayerListItemViewModel? _selectedPlayer;
        public PlayerListItemViewModel? SelectedPlayer
        {
            get
            {
                return _selectedPlayer;
            }
            set
            {
                _selectedPlayer = value;
                OnPropertyChanged(nameof(SelectedPlayer));

            }
        }
        public IEnumerable<SportListItemViewModel> SportList => _sportListItemViewModels;
        public IEnumerable<SubscriptionTrainerListItem> TrainerList => _trainerListItemViewModels;
        public IEnumerable<PlayerListItemViewModel> PlayersList => _playerListItemViewModels;
        public ICommand LoadSportsCommand { get; }
        public ICommand LoadTrainersCommand { get; }
        public ICommand LoadPlayersCommand { get; }
        public ICommand AddSportCommand => new RelayCommand(()=> _dialogService.ShowDialog<SportDetailWindowView, AddSportViewModel>());
        public ICommand ClearTrainerCommand => new RelayCommand(() => SelectedTrainer = null);
        public ICommand AddTrainerCommand => new RelayCommand(()=> _dialogService.ShowDialog<EmployeeDetailsWindowView, AddEmployeeViewModel>());
        public ICommand AddPlayerCommand => new RelayCommand(()=> _dialogService.ShowDialog<PlayerDetailWindowView, AddPlayerViewModel>());
    
        private string? _playerName;
        public string? PlayerName
        {
            get { return _playerName; }
            set { _playerName = value; OnPropertyChanged(nameof(PlayerName)); }
        }
        public CreateSubscriptionWindowViewModel(SportDataStore sportDataStore, SubscriptionDataStore subscriptionStore, PlayersDataStore playerDataStore, PaymentDataStore paymentDataStore, EmployeeStore employeeStore, IDialogService dialogService)
        {
            _sportDataStore = sportDataStore;
            _subscriptionStore = subscriptionStore;
            _playerDataStore = playerDataStore;
            _paymentDataStore = paymentDataStore;
            _employeeStore = employeeStore;

            _employeeStore.Created += _employeeStore_Created;
            _employeeStore.Loaded += _employeeStore_Loaded;
            _sportListItemViewModels = new ObservableCollection<SportListItemViewModel>();
            _trainerListItemViewModels = new ObservableCollection<SubscriptionTrainerListItem>();
            _playerListItemViewModels = new ObservableCollection<PlayerListItemViewModel>();
            _sportDataStore.Loaded += _sportDataStore_Loaded;
            _sportDataStore.Created += _sportDataStore_Created;
            _playerDataStore.Players_loaded += _playerDataStore_Players_loaded;
            _playerDataStore.Player_created += PlayerStore_PlayerAdded;
            _playerDataStore.Player_update += PlayerStore_PlayerUpdated;
            _playerDataStore.Player_deleted += PlayerStore_PlayerDeleted;
            LoadSportsCommand = new LoadSportItemsCommand(_sportDataStore);
            LoadPlayersCommand = new LoadAllPlayersCommand(_playerDataStore);
            SubmitCommand = new CreateMainSubscriptionCommand(_subscriptionStore, this, _playerDataStore, _paymentDataStore);
            LoadTrainersCommand = new LoadTrainersItemCommand(_employeeStore);
            Code = null;
            ClearForm();
            LoadSportsCommand.Execute(null);
            LoadPlayersCommand.Execute(null);
            LoadTrainersCommand.Execute(null);
            _dialogService = dialogService;
        }
        public ICommand? OpenScanCommand { get; }

     
        private void _employeeStore_Loaded()
        {
            _trainerListItemViewModels.Clear();
            foreach (var trainer in _employeeStore.Employees.Where(x => x.IsTrainer))
            {
                AddTrainer(trainer);
            }
            if ((IsRenewal || IsEditMode) && ExistedSubscription != null)
            {
                SelectedTrainer = _trainerListItemViewModels.FirstOrDefault(x => x.Id == ExistedSubscription.TrainerId);
            }
        }

        private void _employeeStore_Created(Core.Models.Employee.Employee obj)
        {
            AddTrainer(obj);
        }

        private void _sportDataStore_Created(Sport obj)
        {
            AddSport(obj);
        }

        public bool IsRenewal { get; set; }
        public bool IsEditMode { get; set; }
        public int? PlayerId { get; set; }
        public void ApplySubscriptionRenew(Subscription subscription)
        {
            IsRenewal = true;
            ExistedSubscription = subscription;
            SubscribeDate = subscription.EndDate.AddDays(1);
            Code = subscription.Code;
            SubscribeDays = subscription.DaysCount;
            if (_playerListItemViewModels.Count > 0)
            {
                SelectedPlayer = _playerListItemViewModels.FirstOrDefault(x => x.Id == ExistedSubscription.PlayerId);
            }
            if (_sportListItemViewModels.Count > 0)
            {
                SelectedSport = _sportListItemViewModels.FirstOrDefault(x => x.Id == ExistedSubscription.SportId);
            }
            if (_trainerListItemViewModels.Count > 0)
            {
                SelectedTrainer = _trainerListItemViewModels.FirstOrDefault(x => x.Id == ExistedSubscription.TrainerId);
            }
        }

        public void ApplySubscriptionEdit(Subscription subscription)
        {
            IsEditMode = true;
            ExistedSubscription = subscription;
            SubscribeDate = subscription.RollDate;
            Code = subscription.Code;
            SubscribeDays = subscription.DaysCount;
            if (_playerListItemViewModels.Count > 0)
            {
                SelectedPlayer = _playerListItemViewModels.FirstOrDefault(x => x.Id == ExistedSubscription.PlayerId);
            }
            if (_sportListItemViewModels.Count > 0)
            {
                SelectedSport = _sportListItemViewModels.FirstOrDefault(x => x.Id == ExistedSubscription.SportId);
            }
            if (_trainerListItemViewModels.Count > 0)
            {
                SelectedTrainer = _trainerListItemViewModels.FirstOrDefault(x => x.Id == ExistedSubscription.TrainerId);
            }
        }
        public void SetPlayer(int? playerId)
        {
            if (playerId != null)
            {
                PlayerId = playerId;
            }
        }
        private void PlayerStore_PlayerDeleted(int id)
        {
            PlayerListItemViewModel? itemViewModel = _playerListItemViewModels.FirstOrDefault(y => y.Player?.Id == id);

            if (itemViewModel != null)
            {
                _playerListItemViewModels.Remove(itemViewModel);
            }
        }

        private void PlayerStore_PlayerUpdated(Player player)
        {
            PlayerListItemViewModel? playerViewModel =
                   _playerListItemViewModels.FirstOrDefault(y => y.Player.Id == player.Id);

            if (playerViewModel != null)
            {
                playerViewModel.Update(player);
            }
        }

        private void PlayerStore_PlayerAdded(Player player)
        {
            AddPlayer(player);
        }

       
        public string? Code { get; set; }

     

        private double _paymentValue;
        public double PaymentValue
        {
            get { return _paymentValue; }
            set
            {
                _paymentValue = value;
                OnPropertyChanged(nameof(PaymentValue));
                ClearError(nameof(PaymentValue));
                if (SelectedSport != null)
                {
                    if (PaymentValue > Total)
                    {
                        AddError("لا يمكن ان يكون المبلغ المدفوع اكبر من المستحق", nameof(PaymentValue));
                        OnErrorChanged(nameof(PaymentValue));
                    }
                    else if (PaymentValue < 0)
                    {
                        AddError("لايمكن الدفع بقيمة اقل من 0", nameof(PaymentValue));
                        OnErrorChanged(nameof(PaymentValue));
                    }
                }

                else
                {
                    AddError("يجب اختيار الاشتراك اولا", nameof(PaymentValue));
                    OnErrorChanged(nameof(PaymentValue));
                }

            }
        }
        private string? _descriptiones;
        public string? Descriptiones
        {
            get { return _descriptiones; }
            set { _descriptiones = value; OnPropertyChanged(nameof(Descriptiones)); }
        }
        private void _playerDataStore_Players_loaded()
        {
            _playerListItemViewModels.Clear();

            foreach (Player player in _playerDataStore.Players)
            {
                AddPlayer(player);
            }
            if ((IsRenewal || IsEditMode) && ExistedSubscription != null)
            {
                SelectedPlayer = _playerListItemViewModels.FirstOrDefault(x => x.Id == ExistedSubscription.PlayerId);
            }
            else if (PlayerId.HasValue)
            {
                SelectedPlayer = _playerListItemViewModels.FirstOrDefault(x => x.Id == PlayerId);
            }
        }


        private void CountTotal()
        {
            if (SelectedSport != null)
            {

                Total = SportPrice - OfferValue;
                PaymentValue = Total ?? 0;
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
        private DateTime _subscribeDate = DateTime.Now.Date;
        public DateTime SubscribeDate
        {
            get { return _subscribeDate; }
            set { _subscribeDate = value; OnPropertyChanged(nameof(SubscribeDate)); }
        }


        public ICommand? SubmitCommand { get; }
        #endregion


        private void _sportDataStore_Loaded()
        {
            _sportListItemViewModels.Clear();

            foreach (Sport sport in _sportDataStore.Sports)
            {
                AddSport(sport);
            }
            if ((IsRenewal || IsEditMode)&&ExistedSubscription!=null)
            {
                SelectedSport = _sportListItemViewModels.FirstOrDefault(x => x.Id == ExistedSubscription.SportId);
            }
            else
                SelectedSport = _sportListItemViewModels.FirstOrDefault();

        }
        public void ClearForm()
        {
            SelectedPlayer = null;
            SubscribeDate = DateTime.Now;
            PaymentValue = 0;
            OfferValue = 0;
            Descriptiones = "";
            Offer = "";
            Code = null;
        }
        private void AddSport(Sport sport)
        {
            SportListItemViewModel itemViewModel =
                new SportListItemViewModel(sport);
            _sportListItemViewModels.Add(itemViewModel);
        }
        private void AddPlayer(Player player)
        {
            PlayerListItemViewModel itemViewModel =
                new PlayerListItemViewModel(player);
            _playerListItemViewModels.Add(itemViewModel);
        }
        public override void Dispose()
        {
           
            _sportDataStore.Loaded -= _sportDataStore_Loaded;
            base.Dispose();
        }
        private void AddTrainer(Core.Models.Employee.Employee trainer)
        {
            SubscriptionTrainerListItem itemViewModel =
                new SubscriptionTrainerListItem(trainer);
            _trainerListItemViewModels.Add(itemViewModel);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue<int>("PlayerId", out var playerId))
            {
                PlayerId = playerId;
                SetPlayer(PlayerId);
            }
            if (query.TryGetValue<Subscription>("Subscription", out var subscription) && subscription != null)
            {
                if (query.TryGetValue<QueryMode>("Mode", out var queryMode))
                {
                    if (queryMode == QueryMode.Renewal)
                    {
                        ApplySubscriptionRenew(subscription);
                    }
                    else
                    {
                        ApplySubscriptionEdit(subscription);
                    }
                }
            }
        }
    }
}
