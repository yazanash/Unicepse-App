using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Sport;
using Unicepse.Commands;
using Unicepse.Commands.Sport;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;
using Unicepse.Stores.SportStores;

namespace Unicepse.ViewModels.SportsViewModels
{
    public class SportListItemViewModel : ViewModelBase
    {
        public Sport Sport;
        private readonly SportDataStore? _sportStore;
        private readonly EmployeeStore? _employeeStore;
        private readonly SportListViewModel? _sportListViewModel;
        private readonly NavigationStore? _navigationStore;
        private SportSubscriptionDataStore? _subscriptionDataStore;
        public int Id => Sport.Id;
        public string? SportName => Sport.Name;
        public double Price => Sport.Price;
        public bool IsActive => Sport.IsActive;
        public int DaysInWeek => Sport.DaysInWeek;
        public double DailyPrice => Sport.DailyPrice;

        public int DaysCount => Sport.DaysCount;

        public ICommand? EditCommand { get; }
        public ICommand? DeleteCommand { get; }
        public ICommand? SubscriptionsCommand { get; }

        public SportListItemViewModel(Sport sport, SportDataStore sportStore, NavigationStore navigationStore, EmployeeStore employeeStore, SportListViewModel sportListViewModel, SportSubscriptionDataStore subscriptionDataStore)
        {
            Sport = sport;
            _sportStore = sportStore;
            _navigationStore = navigationStore;
            _employeeStore = employeeStore;
            _sportListViewModel = sportListViewModel;
            _subscriptionDataStore = subscriptionDataStore;

            EditCommand = new NavaigateCommand<EditSportViewModel>(new NavigationService<EditSportViewModel>(_navigationStore, () => CreateEditSportViewModel(_navigationStore, _sportListViewModel, _sportStore, _employeeStore)));
            DeleteCommand = new DeleteSportCommand(_sportStore);
            SubscriptionsCommand = new NavaigateCommand<SportSubscriptionsViewModel>(new NavigationService<SportSubscriptionsViewModel>(_navigationStore, () => SupscrtionSportViewModel(_sportStore, _subscriptionDataStore)));
        }

        public SportListItemViewModel(Sport sport)
        {
            Sport = sport;
        }

        private SportSubscriptionsViewModel SupscrtionSportViewModel(SportDataStore sportStore, SportSubscriptionDataStore subscriptionDataStore)
        {
            return SportSubscriptionsViewModel.LoadViewModel(sportStore, subscriptionDataStore);
        }
        private EditSportViewModel CreateEditSportViewModel(NavigationStore navigationStore, SportListViewModel sportListViewModel, SportDataStore sportStore, EmployeeStore employeeStore)
        {
            return EditSportViewModel.LoadViewModel(navigationStore, sportListViewModel, sportStore, employeeStore);
        }

        public SportListItemViewModel(Sport sport, SportDataStore sportStore, NavigationStore navigationStore)
        {
            Sport = sport;
            _sportStore = sportStore;
            _navigationStore = navigationStore;
        }
        public void Update(Sport sport)
        {
            Sport = sport;

            OnPropertyChanged(nameof(SportName));
        }
    }
}
