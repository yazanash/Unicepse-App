using PlatinumGym.Core.Models.Player;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.SubscriptionCommand;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using PlatinumGymPro.ViewModels.SubscriptionViewModel;
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
        private readonly NavigationStore _navigationStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly PlayerListViewModel playerListingViewModel;
        public int Id => Player.Id;
        public string? FullName => Player.FullName;
        public string? Phone => Player.Phone;
        public int BirthDate => Player.BirthDate;
        public string Gendertext => Player.GenderMale? "ذكر" : "انثى";
        public bool GenderMale => Player.GenderMale;
        public double Weight => Player.Weight;
        public double Hieght => Player.Hieght;
        public string? SubscribeDate => Player.SubscribeDate.ToString("ddd,MMM dd,yyy");
        public string? SubscribeEndDate => Player.SubscribeEndDate.ToString("ddd,MMM dd,yyy");
        public bool IsTakenContainer => Player.IsTakenContainer;
        public int DayLeft => (int) Player.SubscribeEndDate.Subtract(Player.SubscribeDate).TotalDays;
        public Brush IsSubscribed => Player.IsSubscribed ? Brushes.Green : Brushes.Red;
        public double Balance => Player.Balance;


        public ICommand? EditCommand { get; }
        public ICommand? DeleteCommand { get; }
        public ICommand? SubscriptionCommand { get; }
        public PlayerListItemViewModel(Player player, NavigationStore navigationStore, PlayerListViewModel playerListingViewModel,SubscriptionDataStore subscriptionDataStore)
        {
            Player = player;

            _subscriptionDataStore = subscriptionDataStore;
            _navigationStore = navigationStore;
            this.playerListingViewModel = playerListingViewModel;
            EditCommand = new NavaigateCommand<EditPlayerViewModel>(new NavigationService<EditPlayerViewModel>(_navigationStore, () => new EditPlayerViewModel(_navigationStore,player)));
            SubscriptionCommand = new NavaigateCommand<SubscriptionDetailsViewModel>(new NavigationService<SubscriptionDetailsViewModel>(_navigationStore, () => new SubscriptionDetailsViewModel()));
        }

        public void Update(Player player)
        {
            this.Player = player;

            OnPropertyChanged(nameof(FullName));
        }
       
    }
}
