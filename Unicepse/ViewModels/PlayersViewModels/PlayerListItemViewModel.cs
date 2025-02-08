using Unicepse.Core.Models.Player;
using Unicepse.Commands.Player;
using Unicepse.Commands.PlayerAttendenceCommands;
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
using Unicepse.Stores.RoutineStores;

namespace Unicepse.ViewModels.PlayersViewModels
{
    public class PlayerListItemViewModel : ViewModelBase
    {
        public Player Player;
        private readonly NavigationStore? _navigationStore;
        private readonly PlayersDataStore? _playersDataStore;
        private readonly PlayersAttendenceStore? _playersAttendenceStore;
        private readonly NavigationService<PlayerListViewModel>? _navigationService;
        private readonly HomeViewModel? _homeViewModel;
        private readonly PlayerMainPageViewModel? _playerMainPageViewModel;
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
        public int DayLeft => (int)Player.SubscribeEndDate.Subtract(DateTime.Now).TotalDays + 1;
        public Brush IsSubscribed => Player.IsSubscribed ? Brushes.Green : Brushes.Red;
        public Brush BalanceColor => Balance >= 0 ? Brushes.Green : Brushes.Red;

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
        public ICommand? ReactivePlayerCommand { get; }
        public ICommand? VerifyAccountCommand { get; }

        public PlayerListItemViewModel(Player player, NavigationStore navigationStore,
           PlayersDataStore playersDataStore,
           NavigationService<PlayerListViewModel> navigationService,PlayerMainPageViewModel playerMainPageViewModel)
        {
            Player = player;
            IsVerified = Player.UID != null;
            IsActive = Player.IsSubscribed;
            _playersDataStore = playersDataStore;
            _navigationStore = navigationStore;
            _navigationService = navigationService;
            _playerMainPageViewModel = playerMainPageViewModel;

            VerifyAccountCommand = new VerifyAccountCommand(new ReadPlayerQrCodeViewModel(), _playersDataStore);

            EditCommand = new NavaigateCommand<EditPlayerViewModel>(new NavigationService<EditPlayerViewModel>(_navigationStore, () => new EditPlayerViewModel(_navigationStore, _playersDataStore, _playerMainPageViewModel)));
            DeleteCommand = new DeletePlayerCommand(_navigationService, _playersDataStore);
            ReactivePlayerCommand = new ReactivePlayerCommand(_playersDataStore);
        }

        public PlayerListItemViewModel(Player player, NavigationStore navigationStore, PlayerProfileViewModel playerProfileViewModel)
        {
            Player = player;
            IsVerified = Player.UID != null;
            IsActive = Player.IsSubscribed;
            _navigationStore = navigationStore;
            OpenProfileCommand = new NavaigateCommand<PlayerProfileViewModel>(new NavigationService<PlayerProfileViewModel>(_navigationStore, () => playerProfileViewModel));

        }
        public PlayerListItemViewModel(Player player, NavigationStore navigationStore
           , PlayerProfileViewModel playerProfileViewModel, PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore, HomeViewModel homeViewModel)
        {
            Player = player;
            IsVerified = Player.UID != null;
            IsActive = Player.IsSubscribed;
            _navigationStore = navigationStore;
            _playersAttendenceStore = playersAttendenceStore;
            _playersDataStore = playersDataStore;
            _homeViewModel = homeViewModel;
            OpenProfileCommand = new NavaigateCommand<PlayerProfileViewModel>(new NavigationService<PlayerProfileViewModel>(_navigationStore, () => playerProfileViewModel));

            LogInCommand = new LoginPlayerCommand(_playersAttendenceStore, _playersDataStore, new NavigationService<HomeViewModel>(_navigationStore, () => _homeViewModel), this,playerProfileViewModel,_navigationStore);
        }


        public void Update(Player player)
        {
            Player = player;
            IsVerified = Player.UID != null;
            IsActive = Player.IsSubscribed;
            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(Balance));
        }

    }
}
