using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.RoutinesCommand;
using PlatinumGymPro.Enums;
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
    public class EditSelectRoutineDaysMuscleGroupViewModel : ViewModelBase
    {
        private readonly ObservableCollection<DayGroupListItemViewModel> _dayGroupListItemViewModels;
        public IEnumerable<DayGroupListItemViewModel> DayGroupList => _dayGroupListItemViewModels;

        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly NavigationService<RoutinePlayerViewModels> _navigationService;
        public EditSelectRoutineDaysMuscleGroupViewModel(RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService, PlayersDataStore playersDataStore)
        {
            _routineDataStore = routineDataStore;
            _dayGroupListItemViewModels = new ObservableCollection<DayGroupListItemViewModel>();
            _dayGroupListItemViewModels.Add(new DayGroupListItemViewModel(1, "اليوم الاول"));
            _dayGroupListItemViewModels.Add(new DayGroupListItemViewModel(2, "اليوم الثاني"));
            _dayGroupListItemViewModels.Add(new DayGroupListItemViewModel(3, "اليوم الثالث"));
            _dayGroupListItemViewModels.Add(new DayGroupListItemViewModel(4, "اليوم الرابع"));
            _dayGroupListItemViewModels.Add(new DayGroupListItemViewModel(5, "اليوم الخامس"));
            _dayGroupListItemViewModels.Add(new DayGroupListItemViewModel(6, "اليوم السادس"));
            _navigationService = navigationService;
            _playersDataStore = playersDataStore;

            foreach (var key in _routineDataStore.SelectedRoutine!.DaysGroupMap!)
            {

                DayGroupListItemViewModel? dayGroupListItemViewModel = _dayGroupListItemViewModels.Where(x => x.SelectedDay == key.Key).FirstOrDefault();
                foreach (var value in key.Value)
                {
                    int inval = value;
                    switch (inval)
                    {
                        case (int)EMuscleGroup.Chest:
                            dayGroupListItemViewModel!.Chest = true;
                            break;
                        case (int)EMuscleGroup.Shoulders:
                            dayGroupListItemViewModel!.Shoulders = true;
                            break;
                        case (int)EMuscleGroup.Back:
                            dayGroupListItemViewModel!.Back = true;
                            break;
                        case (int)EMuscleGroup.Legs:
                            dayGroupListItemViewModel!.Legs = true;
                            break;
                        case (int)EMuscleGroup.Biceps:
                            dayGroupListItemViewModel!.Biceps = true;
                            break;
                        case (int)EMuscleGroup.Triceps:
                            dayGroupListItemViewModel!.Triceps = true;
                            break;
                        case (int)EMuscleGroup.Abs:
                            dayGroupListItemViewModel!.Abs = true;
                            break;
                    }
                }
            }

            SubmitCommand = new UpdateRoutineCommand(_routineDataStore, _playersDataStore, _navigationService, this);
            PrintCommand = new PrintCommand(new PrintWindowViewModel(new EditRoutinePrintViewModel(_routineDataStore, _playersDataStore, this), new NavigationStore()));

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
