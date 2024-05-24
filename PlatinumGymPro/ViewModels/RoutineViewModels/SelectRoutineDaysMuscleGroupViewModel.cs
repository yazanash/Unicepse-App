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
    public class SelectRoutineDaysMuscleGroupViewModel : ErrorNotifyViewModelBase
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
            _dayGroupListItemViewModels.Add(new DayGroupListItemViewModel(7, "اليوم السابع"));
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
            Number = _routineDataStore.SelectedRoutine.RoutineNo;
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

        private string? _number;
        public string? Number
        {
            get { return _number; }
            set { _number = value; OnPropertyChanged(nameof(Number));
                ClearError(nameof(Number));
                if (string.IsNullOrEmpty(Number))
                {
                    AddError("هذا الحقل مطلوب", nameof(Number));
                    OnErrorChanged(nameof(Number));
                }
            }
        }
        private bool _isTemplate;
        public bool IsTemplate
        {
            get { return _isTemplate; }
            set { _isTemplate = value;
                OnPropertyChanged(nameof(IsTemplate)); 
                
            }
        }
        public ICommand SubmitCommand { get; }
        public ICommand PrintCommand { get; }

    }
}
