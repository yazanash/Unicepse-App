using PlatinumGym.Core.Models.Sport;
using PlatinumGymPro.Commands.SubscriptionCommand;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.SportsViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.SubscriptionViewModel
{
    public class SubscriptionDetailsViewModel : ViewModelBase
    {
        //private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly SportDataStore _sportDataStore;
        //private readonly EmployeeStore _employeeStore;
        private readonly ObservableCollection<SportListItemViewModel> _sportListItemViewModels;
        private NavigationStore _navigatorStore;
        //private readonly SubscriptionDataStore _subscriptionStore;
        //private readonly PlayersDataStore _playersDataStore;
        public IEnumerable<SportListItemViewModel> SportList => _sportListItemViewModels;
        public ICommand LoadSportsCommand { get; }
        public SubscriptionDetailsViewModel(SportDataStore sportDataStore, NavigationStore navigatorStore)
        {
            _navigatorStore = navigatorStore; 
            _sportDataStore = sportDataStore;
            _sportListItemViewModels = new ObservableCollection<SportListItemViewModel>();
            _sportDataStore.Loaded += _sportDataStore_Loaded;
            LoadSportsCommand = new LoadSportItemsCommand(_sportDataStore);
           
        }

        private void _sportDataStore_Loaded()
        {
            _sportListItemViewModels.Clear();

            foreach (Sport sport in _sportDataStore.Sports)
            {
                AddSport(sport);
            }
        }
        private void AddSport(Sport sport)
        {
            SportListItemViewModel itemViewModel =
                new SportListItemViewModel(sport,_sportDataStore, _navigatorStore);
            _sportListItemViewModels.Add(itemViewModel);
        }


        public static SubscriptionDetailsViewModel LoadViewModel(SportDataStore sportDataStore, NavigationStore navigatorStore)
        {
            SubscriptionDetailsViewModel viewModel = new(sportDataStore, navigatorStore);

            viewModel.LoadSportsCommand.Execute(null);

            return viewModel;
        }
    }
}
