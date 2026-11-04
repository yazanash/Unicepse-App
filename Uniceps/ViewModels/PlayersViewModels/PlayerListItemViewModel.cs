using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Core.Models.Player;
using Uniceps.navigation.DialogService;
using Uniceps.Stores;
using Uniceps.Views.PlayerViews;

namespace Uniceps.ViewModels.PlayersViewModels
{
    public class PlayerListItemViewModel : ViewModelBase
    {
        public Player Player;
        private readonly IDialogService? _dialogService;
        private readonly PlayersDataStore? _playersDataStore;
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
            if (_dialogService != null)
            {
                var navParams = new Dictionary<string, object>
                {
                    { "PlayerId", Player.Id }
                };
                _dialogService.ShowDialog<PlayerProfileWindowView, PlayerProfileViewModel>(navParams);
            }
          

        }
     

        private void OpenFingerprintView()
        {
            if (_dialogService != null)
            {
                var navParams = new Dictionary<string, object>
                {
                    { "Player", Player }
                };
                _dialogService.ShowDialog<CaptureFingerPrintWindowView, EnrollFingerprintViewModel>(navParams);
            }
        }

        public ICommand? CaptureFingerprintCommand { get; }

        private bool _fingerDataAvailable = false;
        public bool FingerDataAvailable
        {
            get => _fingerDataAvailable;
            set { _fingerDataAvailable = value; OnPropertyChanged(nameof(FingerDataAvailable)); }
        }

        public PlayerListItemViewModel(Player player)
        {
            Player = player;
            IsVerified = Player.UID != null;
        }
        public PlayerListItemViewModel(Player player,
        PlayersDataStore playersDataStore, IDialogService dialogService)
        {
            Player = player;
            IsVerified = Player.UID != null;
            _playersDataStore = playersDataStore;
            EditCommand = new RelayCommand(ExecuteOpenEditCommand);
            DeleteCommand = new DeletePlayerCommand(_playersDataStore);
            CaptureFingerprintCommand = new RelayCommand(OpenFingerprintView);
            FingerDataAvailable = string.IsNullOrEmpty(Player.FingerprintData);
            _dialogService = dialogService;
        }
        public void ExecuteOpenEditCommand()
        {
            if (_dialogService != null)
            {
                var navParams = new Dictionary<string, object>
                {
                    { "Player", Player }
                };
                _dialogService.ShowDialog<PlayerDetailWindowView, AddPlayerViewModel>(navParams);
            }
        }
        

        public void Update(Player player)
        {
            Player = player;
            IsVerified = Player.UID != null;
            FingerDataAvailable = string.IsNullOrEmpty(Player.FingerprintData);
            OnPropertyChanged(nameof(FullName));
            OnPropertyChanged(nameof(Balance));
            OnPropertyChanged(nameof(FingerDataAvailable));
        }

    }
}
