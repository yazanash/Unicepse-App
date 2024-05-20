using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.RoutinesCommand;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.PrintViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.RoutineViewModels
{
    public class SelectRoutineDaysMuscleGroupViewModel : ViewModelBase
    {
        private readonly ObservableCollection<DayGroupListItemViewModel> _dayGroupListItemViewModels;
        public IEnumerable<DayGroupListItemViewModel> DayGroupList => _dayGroupListItemViewModels;

        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly NavigationService<RoutinePlayerViewModels> _navigationService;
        public SelectRoutineDaysMuscleGroupViewModel(RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService, PlayersDataStore playersDataStore)
        {
            _routineDataStore = routineDataStore;
            _dayGroupListItemViewModels = new ObservableCollection<DayGroupListItemViewModel>();
            _dayGroupListItemViewModels.Add(new DayGroupListItemViewModel(1, "اليوم الاول"));
            _dayGroupListItemViewModels.Add(new DayGroupListItemViewModel(2, "اليوم الثاني"));
            _dayGroupListItemViewModels.Add(new DayGroupListItemViewModel(3, "اليوم الثالث"));
            _dayGroupListItemViewModels.Add(new DayGroupListItemViewModel(4, "اليوم الرابع"));
            _dayGroupListItemViewModels.Add(new DayGroupListItemViewModel(5, "اليوم الخامس"));
            _dayGroupListItemViewModels.Add(new DayGroupListItemViewModel(6,"اليوم السادس"));
            _navigationService = navigationService;
            _playersDataStore = playersDataStore;
            SubmitCommand = new SubmitRoutineCommand(_routineDataStore, _playersDataStore, _navigationService,this);
            string filename = _playersDataStore.SelectedPlayer!.FullName + "_" + Date.ToShortDateString() + "_Routine";
            PrintCommand = new PrintCommand(new PrintWindowViewModel(new RoutinePrintViewModel(_routineDataStore,_playersDataStore,this), new NavigationStore()),filename);
        }
        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(nameof(Date)); }
        }

        private int _number;
        public int Number
        {
            get { return _number; }
            set { _number = value; OnPropertyChanged(nameof(Number)); }
        }
        public ICommand SubmitCommand { get; }
        public ICommand PrintCommand { get; }

    }
}
