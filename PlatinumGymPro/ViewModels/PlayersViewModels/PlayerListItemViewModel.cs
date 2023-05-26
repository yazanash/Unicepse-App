using PlatinumGymPro.Commands;
using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using PlatinumGymPro.ViewModels.TrainingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace PlatinumGymPro.ViewModels.PlayersViewModels
{
    public class PlayerListItemViewModel : ViewModelBase
    {
        public Player Player;
        private readonly PlayerStore _playerStore;
        private readonly NavigationStore _navigationStore;
        private readonly TrainerStore _trainerStore;
        private readonly SportStore _sportStore;
        private readonly PlayerListViewModel playerListingViewModel;
        public int Id => Player.Id;
        public string? FullName => Player.FullName;
        public string? Phone => Player.Phone;
        public int BirthDate => Player.BirthDate;
        public bool GenderMale => Player.GenderMale;
        public double Weight => Player.Weight;
        public double Hieght => Player.Hieght;
        public string? SubscribeDate => Player.SubscribeDate.ToShortDateString();
        public string? SubscribeEndDate => Player.SubscribeEndDate.ToShortDateString();
        public bool IsTakenContainer => Player.IsTakenContainer;
        public Brush IsSubscribed => Player.IsSubscribed ? Brushes.Green : Brushes.Red;
        public double Balance => Player.Balance;


        public ICommand? EditCommand { get; }
        public ICommand? DeleteCommand { get; }

        public PlayerListItemViewModel(Player player, PlayerStore playerStore, NavigationStore navigationStore, TrainerStore trainerStore, SportStore sportStore, PlayerListViewModel playerListingViewModel)
        {
            Player = player;

            _playerStore = playerStore;
            _navigationStore = navigationStore;
            _trainerStore = trainerStore;
            this.playerListingViewModel = playerListingViewModel;
            _sportStore = sportStore;
            EditCommand = new NavaigateCommand<AddTrainingViewModel>(new NavigationService<AddTrainingViewModel>(_navigationStore, () => CreateAddSportViewModel(_navigationStore, _sportStore, playerListingViewModel, _trainerStore)));
           
        }

        public void Update(Player player)
        {
            this.Player = player;

            OnPropertyChanged(nameof(FullName));
        }
        private AddTrainingViewModel CreateAddSportViewModel(NavigationStore navigatorStore, SportStore _sportStore, PlayerListViewModel playerListingViewModel, TrainerStore trainerStore)
        {
            return AddTrainingViewModel.LoadViewModel(_sportStore, navigatorStore, playerListingViewModel, trainerStore);
        }
    }
}
