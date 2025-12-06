using Uniceps.Core.Models.Employee;
using Uniceps.Commands;
using Uniceps.Commands.Sport;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Stores;
using Uniceps.navigation.Stores;
using Uniceps.Stores.SportStores;
using Uniceps.Core.Models.Sport;
using Uniceps.Views.SportViews;

namespace Uniceps.ViewModels.SportsViewModels
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

            EditCommand = new RelayCommand(ExecuteEditSportCommand);
            DeleteCommand = new DeleteSportCommand(_sportStore);
            SubscriptionsCommand = new NavaigateCommand<SportSubscriptionsViewModel>(new NavigationService<SportSubscriptionsViewModel>(_navigationStore, () => SupscrtionSportViewModel(_sportStore, _subscriptionDataStore)));
        }
        public void ExecuteEditSportCommand()
        {
            EditSportViewModel editSportViewModel = EditSportViewModel.LoadViewModel(_sportStore!, _employeeStore!);
            SportDetailWindowView sportDetailWindowView = new SportDetailWindowView();
            sportDetailWindowView.DataContext = editSportViewModel;
            sportDetailWindowView.ShowDialog();
        }
        public SportListItemViewModel(Sport sport)
        {
            Sport = sport;
        }

        private SportSubscriptionsViewModel SupscrtionSportViewModel(SportDataStore sportStore, SportSubscriptionDataStore subscriptionDataStore)
        {
            return SportSubscriptionsViewModel.LoadViewModel(sportStore, subscriptionDataStore);
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
