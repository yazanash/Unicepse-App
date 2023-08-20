using PlatinumGymPro.Commands;
using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using PlatinumGymPro.ViewModels.HomePageViewModels;
using PlatinumGymPro.ViewModels.PlayersViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly ObservableCollection<PlayerListItemViewModel> playerListItemViewModels;
        private PlayerStore _playerStore;
        private TrainerStore _trainerStore;
        public ObservableCollection<PlayerListItemViewModel> Players { get; }
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
            set
            {
                _isLoaded = value;
                OnPropertyChanged(nameof(IsLoaded));
            }
        }
        public HomeViewModel(PlayerStore playerStore, TrainerStore trainerStore)
        {
            _playerStore = playerStore;
            _trainerStore = trainerStore;
            //LoadPlayersCommand = new LoadPlayersCommand(_playerStore, this);
            Players = new();

        }




    }
}
