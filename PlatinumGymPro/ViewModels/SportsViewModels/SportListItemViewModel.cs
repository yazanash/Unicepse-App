using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Models.Sport;
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
