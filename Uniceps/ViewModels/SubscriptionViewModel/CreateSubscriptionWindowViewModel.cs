using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Commands.SubscriptionCommand;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.TrainersViewModels;
using Uniceps.ViewModels.PaymentsViewModels;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.SportsViewModels;
using Uniceps.Views.EmployeeViews;
using Uniceps.Views.SportViews;
using Uniceps.Views.SubscriptionView;

namespace Uniceps.ViewModels.SubscriptionViewModel
{
    public class CreateSubscriptionWindowViewModel : ErrorNotifyViewModelBase
    {
        private readonly SportDataStore _sportDataStore;
        private readonly ObservableCollection<SportListItemViewModel> _sportListItemViewModels;
        private readonly ObservableCollection<SubscriptionTrainerListItem> _trainerListItemViewModels;
        private readonly ObservableCollection<PlayerListItemViewModel> _playerListItemViewModels;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PlayersDataStore _playerDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly EmployeeStore _employeeStore;
        public ObservableCollection<Year> years;

        public IEnumerable<Year> Years => years;

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
                    GetTrainers(SelectedSport.Sport);
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
                if (SelectedPlayer != null)
                {
                    Phone = SelectedPlayer.Phone;
                    Year = years.FirstOrDefault(x => x.year == SelectedPlayer.BirthDate);
                    GenderMale = SelectedPlayer.GenderMale;

                }
                else
                {
                    Phone = "";
                    Year = years.FirstOrDefault(); ;

                }

                OnPropertyChanged(nameof(SelectedPlayer));

            }
        }
        public IEnumerable<SportListItemViewModel> SportList => _sportListItemViewModels;
        public IEnumerable<SubscriptionTrainerListItem> TrainerList => _trainerListItemViewModels;
        public IEnumerable<PlayerListItemViewModel> PlayersList => _playerListItemViewModels;
        public ICommand LoadSportsCommand { get; }
        public ICommand LoadPlayersCommand { get; }
        public ICommand AddSportCommand => new RelayCommand(ExecuteAddSportCommand);
        public ICommand ClearTrainerCommand => new RelayCommand(ExecuteClearTrainerCommand);
        private void ExecuteAddSportCommand()
        {
            AddSportViewModel addSportViewModel = AddSportViewModel.LoadViewModel(_sportDataStore, _employeeStore);
            SportDetailWindowView sportDetailWindow = new SportDetailWindowView();
            sportDetailWindow.DataContext = addSportViewModel;
            sportDetailWindow.ShowDialog();
        }
        private void ExecuteClearTrainerCommand()
        {
            SelectedTrainer = null;
        }
        public ICommand AddTrainerCommand => new RelayCommand(ExecuteAddTrainerCommand);
        private void ExecuteAddTrainerCommand()
        {
            AddTrainerViewModel addTrainerViewModel = AddTrainerViewModel.LoadViewModel(_sportDataStore, _employeeStore);
            TrainerDetailsWindowView trainerDetailsWindow = new TrainerDetailsWindowView();
            trainerDetailsWindow.DataContext = addTrainerViewModel;
            trainerDetailsWindow.ShowDialog();
        }
        private string? _playerName;
        public string? PlayerName
        {
            get { return _playerName; }
            set { _playerName = value; OnPropertyChanged(nameof(PlayerName)); }
        }
        public CreateSubscriptionWindowViewModel(SportDataStore sportDataStore, SubscriptionDataStore subscriptionStore, PlayersDataStore playerDataStore, PaymentDataStore paymentDataStore, EmployeeStore employeeStore)
        {
            years = new ObservableCollection<Year>();
            for (int i = DateTime.Now.Year; i > DateTime.Now.Year - 80; i--)
                years.Add(new Year() { year = i });
            _sportDataStore = sportDataStore;
            _subscriptionStore = subscriptionStore;
            _playerDataStore = playerDataStore;
            _paymentDataStore = paymentDataStore;
            _employeeStore = employeeStore;
            _employeeStore.Created += _employeeStore_Created;
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
            Code = null;
        }

        private void _employeeStore_Created(Core.Models.Employee.Employee obj)
        {
            if (obj.Sports!.Any(x => x.Id == SelectedSport!.Id))
            {
                AddTrainer(obj);
            }
        }

        private void _sportDataStore_Created(Sport obj)
        {
            AddSport(obj);
        }

        public bool IsRenewal { get; set; }
        public bool IsPlayerSet { get; set; }
        public int RenewedSubscriptionId { get; set; }
        public int RenewedSubscriptionSportId { get; set; }
        public int RenewedSubscriptionPlayerId { get; set; }
        public int RenewedSubscriptionTrainerId { get; set; }
        public void ApplySubscriptionRenew(Subscription subscription)
        {
            IsRenewal = true;
            RenewedSubscriptionId = subscription.Id;
            RenewedSubscriptionPlayerId = subscription.PlayerId;
            RenewedSubscriptionSportId = subscription.SportId;
            RenewedSubscriptionTrainerId = subscription.PlayerId;
            Code = subscription.Code;
            if (_playerListItemViewModels.Count() > 0)
            {
                SelectedPlayer = _playerListItemViewModels.FirstOrDefault(x => x.Id == RenewedSubscriptionPlayerId);
            }
            if (_sportListItemViewModels.Count() > 0)
            {
                SelectedSport = _sportListItemViewModels.FirstOrDefault(x => x.Id == RenewedSubscriptionSportId);
            }
            SubscribeDate = subscription.EndDate.AddDays(1);
            SubscribeDays = subscription.DaysCount;
        }
        public void SetPlayer(Player player)
        {
            IsPlayerSet = true;
            RenewedSubscriptionPlayerId = player.Id;
            if (_playerListItemViewModels.Count() > 0)
            {

                SelectedPlayer = _playerListItemViewModels.FirstOrDefault(x => x.Id == player.Id);
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

        private string? _phone = "0";
        public string? Phone
        {
            get { return _phone; }
            set
            {
                _phone = value; OnPropertyChanged(nameof(Phone));
                ClearError(nameof(Phone));
                if (Phone?.Trim().Length < 10)
                {
                    AddError("يجب ان يكون رقم الهاتف 10 ارقام", nameof(Phone));
                    OnErrorChanged(nameof(Phone));
                }

            }
        }
        public string? Code { get; set; }

        private Year? _year;
        public Year? Year
        {
            get { return _year; }
            set
            {
                _year = value;

                OnPropertyChanged(nameof(Year));
            }
        }
        private bool _genderMale;
        public bool GenderMale
        {
            get { return _genderMale; }
            set { _genderMale = value; OnPropertyChanged(nameof(GenderMale)); }
        }

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
            if (IsRenewal && SelectedPlayer != null)
            {
                SelectedPlayer = _playerListItemViewModels.FirstOrDefault(x => x.Id == RenewedSubscriptionPlayerId);
            }
            if (IsPlayerSet && SelectedPlayer != null)
            {
                SelectedPlayer = _playerListItemViewModels.FirstOrDefault(x => x.Id == RenewedSubscriptionPlayerId);
            }
        }

        private void GetTrainers(Sport? sport)
        {
            _trainerListItemViewModels.Clear();
            if (sport != null)
                foreach (var trainer in sport!.Trainers!)
                {
                    AddTrainer(trainer);
                }
            if (IsRenewal && SelectedSport != null && SelectedSport.Id == RenewedSubscriptionSportId)
            {
                SelectedTrainer = _trainerListItemViewModels.FirstOrDefault(x => x.Id == RenewedSubscriptionTrainerId);
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
            if (IsRenewal && SelectedSport != null)
            {
                SelectedSport = _sportListItemViewModels.FirstOrDefault(x => x.Id == RenewedSubscriptionSportId);
            }
            else if (SelectedSport != null)
            {
                SelectedSport = _sportListItemViewModels.FirstOrDefault(x => x.Id == SelectedSport.Id);
            }
            else
                SelectedSport = _sportListItemViewModels.FirstOrDefault();

        }
        public void ClearForm()
        {
            PlayerName = "";
            SelectedPlayer = null;
            Phone = "";
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
        public static CreateSubscriptionWindowViewModel LoadViewModel(SportDataStore sportDataStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, PaymentDataStore paymentDataStore, EmployeeStore employeeStore)
        {
            CreateSubscriptionWindowViewModel viewModel = new(sportDataStore, subscriptionDataStore, playersDataStore, paymentDataStore, employeeStore);
            viewModel.LoadSportsCommand.Execute(null);
            viewModel.LoadPlayersCommand.Execute(null);
            return viewModel;
        }
    }
}
