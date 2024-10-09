using Unicepse.Core.Models.Player;
using Unicepse.Commands;
using Unicepse.Commands.Player;
using Unicepse.Commands.PlayerAttendenceCommands;
using Unicepse.Commands.SubscriptionCommand;
using Unicepse.ViewModels.SubscriptionViewModel;
using Unicepse.ViewModels.TrainingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Unicepse.navigation;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;
using Unicepse.ViewModels.RoutineViewModels;

namespace Unicepse.ViewModels.PlayersViewModels
{
    public class PlayerListItemViewModel : ViewModelBase
    {
        public Player Player;
        private readonly NavigationStore? _navigationStore;
        private readonly SubscriptionDataStore? _subscriptionDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly SportDataStore? _sportDataStore;
        private readonly PaymentDataStore? _paymentDataStore;
        private readonly MetricDataStore? _metricDataStore;
        private readonly RoutineDataStore? _routineDataStore;

        private readonly PlayersAttendenceStore? _playersAttendenceStore;
        public int Id => Player.Id;

        private int _order;
        public int Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged(nameof(Order)); }
        }

        public string? FullName => Player.FullName;
        public string? Phone => Player.Phone;
        public int BirthDate => Player.BirthDate;
        public string Gendertext => Player.GenderMale ? "ذكر" : "انثى";
        public bool GenderMale => Player.GenderMale;
        public double Weight => Player.Weight;
        public double Hieght => Player.Hieght;
        public string? SubscribeDate => Player.SubscribeDate.ToShortDateString();
        public string? SubscribeEndDate => Player.SubscribeEndDate.ToShortDateString();
        public bool IsTakenContainer => Player.IsTakenContainer;
        public int DayLeft => (int)Player.SubscribeEndDate.Subtract(DateTime.Now).TotalDays +1;
        public Brush IsSubscribed => Player.IsSubscribed ? Brushes.Green : Brushes.Red; 

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged(nameof(IsActive)); }
        }

        public double Balance => Player.Balance;

        private bool _isVerified;
        public bool IsVerified
        {
            get { return _isVerified; }
            set { _isVerified = value; OnPropertyChanged(nameof(IsVerified)); }
        }

        public ICommand? EditCommand { get; }
        public ICommand? DeleteCommand { get; }
        public ICommand? TrainingProgramCommand { get; }
        public ICommand? OpenProfileCommand { get; }
        public ICommand? LogInCommand { get; }
        public ICommand?  ReactivePlayerCommand { get; }
        public ICommand? VerifyAccountCommand { get; }

        public PlayerListItemViewModel(Player player, NavigationStore navigationStore,
            SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore,
            SportDataStore sportDataStore, PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, PlayerListViewModel playerList, PlayersAttendenceStore playersAttendenceStore)
        {
            Player = player;
            IsVerified = Player.UID != null;
            IsActive = Player.IsSubscribed; 
            _playersDataStore = playersDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _navigationStore = navigationStore;
            _sportDataStore = sportDataStore;
            _paymentDataStore = paymentDataStore;
            _metricDataStore = metricDataStore;
            _routineDataStore = routineDataStore;
            _playersAttendenceStore = playersAttendenceStore;
            VerifyAccountCommand = new VerifyAccountCommand(new ReadPlayerQrCodeViewModel(),_playersDataStore);
            NavigationStore PlayerMainPageNavigation = new NavigationStore();
            EditCommand = new NavaigateCommand<EditPlayerViewModel>(new NavigationService<EditPlayerViewModel>(PlayerMainPageNavigation, () => new EditPlayerViewModel(PlayerMainPageNavigation, _playersDataStore, _subscriptionDataStore, CreatePlayerMainPageViewModel(PlayerMainPageNavigation, _subscriptionDataStore, _playersDataStore, _paymentDataStore, _sportDataStore), _sportDataStore, _paymentDataStore)));
            DeleteCommand = new DeletePlayerCommand(new NavigationService<PlayerListViewModel>(_navigationStore, () => playerList), _playersDataStore);
            OpenProfileCommand = new NavaigateCommand<PlayerProfileViewModel>(new NavigationService<PlayerProfileViewModel>(_navigationStore, () => CreatePlayerProfileViewModel(PlayerMainPageNavigation, _subscriptionDataStore, _playersDataStore, _sportDataStore, _paymentDataStore, _metricDataStore, _routineDataStore, _playersAttendenceStore)));
            TrainingProgramCommand = new NavaigateCommand<RoutinePlayerViewModels>(new NavigationService<RoutinePlayerViewModels>(_navigationStore, () => LoadRoutineViewModel(_routineDataStore, _playersDataStore, _navigationStore)));
            ReactivePlayerCommand = new ReactivePlayerCommand(new NavigationService<PlayerListViewModel>(_navigationStore, () => playerList), _playersDataStore);
        }

        //public PlayerListItemViewModel(Player player, PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore, NavigationStore navigationStore,HomeViewModel homeViewModel)
        //{
        //    Player = player;
        //    _playersDataStore = playersDataStore;
        //    _playersAttendenceStore = playersAttendenceStore;
        //    _navigationStore = navigationStore;
        //    _homeViewModel = homeViewModel;
        //    LogInCommand = new LoginPlayerCommand(_playersAttendenceStore, _subscriptionDataStore, new NavigationService<HomeViewModel>(_navigationStore, () => _homeViewModel));
        //}
        public void Update(Player player)
        {
            Player = player;

            OnPropertyChanged(nameof(FullName));
        }
        private static PlayerProfileViewModel CreatePlayerProfileViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore, MetricDataStore _metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore)
        {
            //playersDataStore.SelectedPlayer = this;
            return new PlayerProfileViewModel(navigatorStore, subscriptionDataStore, playersDataStore, sportDataStore, paymentDataStore, _metricDataStore, routineDataStore, playersAttendenceStore);
        }
        private static PlayerMainPageViewModel CreatePlayerMainPageViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, PaymentDataStore paymentDataStore, SportDataStore sportDataStore)
        {
            //playersDataStore.SelectedPlayer = this
            return new PlayerMainPageViewModel(navigatorStore, subscriptionDataStore, playersDataStore, paymentDataStore, sportDataStore);
        }
        private RoutinePlayerViewModels LoadRoutineViewModel(RoutineDataStore routineDataStore, PlayersDataStore playerDataStore, NavigationStore navigationStore)
        {
            return RoutinePlayerViewModels.LoadViewModel(routineDataStore, playerDataStore, navigationStore);
        }
    }
}
