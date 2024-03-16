using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Models.Sport;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.Sport;
using PlatinumGymPro.Services;
//using PlatinumGymPro.Models;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.SportsViewModels
{
    public class SportListItemViewModel : ViewModelBase
    {
        public Sport Sport;
        private readonly SportDataStore _sportStore;
        private readonly EmployeeStore?  _employeeStore;
        private readonly SportListViewModel? _sportListViewModel;
        private readonly NavigationStore _navigationStore;
        public int Id => Sport.Id;
        public string? SportName => Sport.Name;
        public double Price => Sport.Price;
        public bool IsActive => Sport.IsActive;
        public int DaysInWeek => Sport.DaysInWeek;
        public double DailyPrice => Sport.DailyPrice;
       
        public int DaysCount => Sport.DaysCount;

        public ICommand? EditCommand { get; }
        public ICommand? DeleteCommand { get; }

        public SportListItemViewModel(Sport sport, SportDataStore sportStore, NavigationStore navigationStore, EmployeeStore employeeStore, SportListViewModel sportListViewModel)
        {
            Sport = sport;
            _sportStore = sportStore;
            _navigationStore = navigationStore;
            _employeeStore = employeeStore;
            _sportListViewModel = sportListViewModel;
            EditCommand = new NavaigateCommand<EditSportViewModel>(new NavigationService<EditSportViewModel>(_navigationStore, () => CreateEditSportViewModel(_navigationStore, _sportListViewModel, _sportStore, _employeeStore)));
            DeleteCommand = new DeleteSportCommand(_sportStore);
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
            this.Sport = sport;

            OnPropertyChanged(nameof(SportName));
        }
    }
}
