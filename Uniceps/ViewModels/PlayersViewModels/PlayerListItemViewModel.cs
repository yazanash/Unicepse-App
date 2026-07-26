using Uniceps.Commands.Player;
using Uniceps.Commands.PlayerAttendenceCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Uniceps.Stores.RoutineStores;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.navigation.Stores;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.Core.Models.Player;
using Uniceps.Views.PlayerViews;
using Uniceps.Commands;

namespace Uniceps.ViewModels.PlayersViewModels
{
    public class PlayerListItemViewModel : ViewModelBase
    {
        public Player Player;
        private readonly PlayersDataStore? _playersDataStore;
        private readonly Func<int, PlayerProfileViewModel>? _playerProfileFactory;
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
        public string? MediclStatus => Player.MediclStatus;
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
        public ICommand? OpenProfileCommand => new RelayCommand(ExecuteOpenPlayerProfile);
        public ICommand? CatchFingerPrint { get; }
        private void ExecuteOpenPlayerProfile()
        {
            if (_playerProfileFactory != null)
            {
                PlayerProfileWindowView playerProfileWindowView = new PlayerProfileWindowView();
                playerProfileWindowView.DataContext = _playerProfileFactory(Id);
                playerProfileWindowView.ShowDialog();
            }

        }
        public PlayerListItemViewModel(Player player, 
           PlayersDataStore playersDataStore)
        {
            Player = player;
            IsVerified = Player.UID != null;
            IsActive = Player.IsSubscribed;
            _playersDataStore = playersDataStore;
            EditCommand = new RelayCommand(ExecuteOpenEditCommand);
            DeleteCommand = new DeletePlayerCommand(_playersDataStore);

        }

        private bool _fingerDataAvailable = true;
        public bool FingerDataAvailable
        {
            get => _fingerDataAvailable;
            set { _fingerDataAvailable = value; OnPropertyChanged(nameof(FingerDataAvailable)); }
        }

        public PlayerListItemViewModel(Player player, Func<int, PlayerProfileViewModel> playerProfileFactory)
        {
            Player = player;
            IsVerified = Player.UID != null;
            IsActive = Player.IsSubscribed;
            _playerProfileFactory = playerProfileFactory;

        }
        public void ExecuteOpenEditCommand()
        {
            AddPlayerViewModel editPlayerViewModel = new AddPlayerViewModel(Player,_playersDataStore!);
            PlayerDetailWindowView playerDetailWindowView = new PlayerDetailWindowView();
            playerDetailWindowView.DataContext = editPlayerViewModel;
            playerDetailWindowView.ShowDialog();
        }
        public PlayerListItemViewModel(Player player)
        {
            Player = player;

        }
        public PlayerListItemViewModel(Player player , Func<int, PlayerProfileViewModel> playerProfileFactory, PlayersDataStore playersDataStore, PlayersAttendenceStore playersAttendenceStore, HomeViewModel homeViewModel)
        {
            Player = player;
            IsVerified = Player.UID != null;
            IsActive = Player.IsSubscribed;
            _playersDataStore = playersDataStore;
            _playerProfileFactory = playerProfileFactory;
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
