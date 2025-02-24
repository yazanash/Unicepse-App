using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Unicepse.Commands.RoutinesCommand;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.navigation.Stores;
using Unicepse.navigation;
using Unicepse.Stores;
using Unicepse.Stores.RoutineStores;
using Unicepse.utlis.common;
using Unicepse.ViewModels.RoutineViewModels;

namespace Unicepse.ViewModels
{
    public class RoutineTemplateViewModel : ListingViewModelBase
    {
        private readonly ExercisesDataStore _exercisesDataStore;
        private readonly ObservableCollection<ExercisesListItemViewModel> _exercisesListItemViewModel;

        private readonly ObservableCollection<DailyList> _dailyLists;
        private readonly ObservableCollection<GroupList> _groupLists;

        public IEnumerable<ExercisesListItemViewModel> ExercisesList => _exercisesListItemViewModel;
        public IEnumerable<DailyList> DailyLists => _dailyLists;
        public IEnumerable<GroupList> GroupList => _groupLists;

        public ICommand AddDailyListCommand { get; set; }
        public ICommand LoadExercisesCommand { get; set; }
        public RoutineTemplateViewModel(ExercisesDataStore exercisesDataStore)
        {
            _exercisesDataStore = exercisesDataStore;
            LoadExercisesCommand = new LoadExercisesCommand(_exercisesDataStore, this);
            _exercisesListItemViewModel = new ObservableCollection<ExercisesListItemViewModel>();
            _dailyLists = new ObservableCollection<DailyList>();
            _groupLists = new ObservableCollection<GroupList>();
            _groupLists.Add(new GroupList
            {
                Name = "معدة",
                Id = (int)EMuscleGroup.Abs
            });
            _groupLists.Add(new GroupList
            {
                Name = "ظهر",
                Id = (int)EMuscleGroup.Back
            });
            _groupLists.Add(new GroupList
            {
                Name = "ترايسبس",
                Id = (int)EMuscleGroup.Triceps
            });
            _groupLists.Add(new GroupList
            {
                Name = "بايسيبس",
                Id = (int)EMuscleGroup.Biceps
            });
            _groupLists.Add(new GroupList
            {
                Name = "الأكتاف",
                Id = (int)EMuscleGroup.Shoulders
            });
            _groupLists.Add(new GroupList
            {
                Name = "الارجل",
                Id = (int)EMuscleGroup.Legs
            });
            _groupLists.Add(new GroupList
            {
                Name = "بطات الارجل",
                Id = (int)EMuscleGroup.Calves
            });
            _groupLists.Add(new GroupList
            {
                Name = "الصدر",
                Id = (int)EMuscleGroup.Chest,
            });
            _exercisesDataStore.ExercisesLoaded += _routineDataStore_ExercisesLoaded;
           

            AddDailyListCommand = new RelayCommand(AddDailyList);
        }
        private void _routineDataStore_ExercisesLoaded()
        {
           
                foreach (var exercise in _exercisesDataStore.Exercises)
                {

                    AddExercise(exercise);
                }
        }
        private void AddExercise(Exercises exercise)
        {
           
                ExercisesListItemViewModel exercisesListItemViewModel = new ExercisesListItemViewModel(exercise);
                _exercisesListItemViewModel.Add(exercisesListItemViewModel);
            _groupLists.SingleOrDefault(x => x.Id == exercise.GroupId)!.Exercises.Add(exercisesListItemViewModel);
        }
        private void AddDailyList(object obj)
        {

            _dailyLists.Add(new DailyList { Day = "Day "+(_dailyLists.Count +1) });
        }
        public static RoutineTemplateViewModel LoadViewModel(ExercisesDataStore exercisesDataStore)
        {
            RoutineTemplateViewModel viewModel = new(exercisesDataStore);

            viewModel.LoadExercisesCommand.Execute(null);

            return viewModel;
        }
    }
    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
    public class DailyList
    {
        public string Day { get; set; }
        public ObservableCollection<RoutineItemFillViewModel> Exercises { get; set; } = new ObservableCollection<RoutineItemFillViewModel>();
    }
    public class GroupList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<ExercisesListItemViewModel> Exercises { get; set; } = new ObservableCollection<ExercisesListItemViewModel>();
    }
}
