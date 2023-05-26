using PlatinumGymPro.Commands;
using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels
{
    public class PlayerListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<PlayerViewModel> _Players;
        private readonly NavigationService<MakePlayerViewModel> _navigationService;
        public ObservableCollection<PlayerViewModel> Players => _Players;
        public ICommand? AddPlayerCommand { get; }
        public ICommand? LoadPlayersCommand { get; }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }
        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);
        private bool _isLoaded;
        public bool IsLoaded
        {
             get => _isLoaded;
            set { _isLoaded = value;
                OnPropertyChanged(nameof(IsLoaded));
            }
        }
        public PlayerListingViewModel( NavigationService<MakePlayerViewModel> navigationService, GymStore gymStore)
        {
            _navigationService = navigationService;
            _Players = new ObservableCollection<PlayerViewModel>();
            AddPlayerCommand = new NavaigateCommand<MakePlayerViewModel>(_navigationService);
            //LoadPlayersCommand = new LoadPlayersCommand(gymStore, this);
            gymStore.PlayerMade += GymStore_PlayerMade;
        }

        private void GymStore_PlayerMade(Player player)
        {
            PlayerViewModel playerViewModel = new(player);
            _Players.Add(playerViewModel);
        }

        public static PlayerListingViewModel? LoadViewModel(NavigationService<MakePlayerViewModel> navigationService, GymStore gymStore)
        {
            PlayerListingViewModel viewModel = new(navigationService, gymStore);
             viewModel?.LoadPlayersCommand?.Execute(null);
            return viewModel;
        }
        public void UpdatePlayersList(IEnumerable<Player> players)
        {
            _Players.Clear();
            foreach (Player player in players )
            {
                PlayerViewModel playerViewModel = new(player);
                _Players.Add(playerViewModel);
            }
        }
    }
}
