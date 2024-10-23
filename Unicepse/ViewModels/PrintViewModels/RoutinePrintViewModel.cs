using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.ViewModels.RoutineViewModels;
using Unicepse.utlis.common;
using Unicepse.Stores;
using System.Windows.Media.Imaging;

namespace Unicepse.ViewModels.PrintViewModels
{
    public class RoutinePrintViewModel : ViewModelBase
    {
        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly SelectRoutineDaysMuscleGroupViewModel _selectRoutineDaysMuscleGroupViewModel;
        private readonly LicenseDataStore _licenseDataStore;
        public RoutinePrintViewModel(RoutineDataStore routineDataStore, PlayersDataStore playersDataStore, SelectRoutineDaysMuscleGroupViewModel selectRoutineDaysMuscleGroupViewModel, LicenseDataStore licenseDataStore)
        {
            _routineDataStore = routineDataStore;
            _playersDataStore = playersDataStore;
            _routineDataStore.daysItemCreated += _routineDataStore_daysItemCreated;
            _routineDataStore.daysItemDeleted += _routineDataStore_daysItemDeleted;
            _selectRoutineDaysMuscleGroupViewModel = selectRoutineDaysMuscleGroupViewModel;
            _routineExercisesItemsViewModels = new ObservableCollection<RoutineExercisesItemsViewModel>();
            _dayGroupListItemViewModels = new ObservableCollection<DayGroupListItemViewModel>();
            foreach (var routine in _routineDataStore.RoutineItems)
            {
                RoutineExercisesItemsViewModel routineExercisesItemsViewModel = new(routine);
                _routineExercisesItemsViewModels.Add(routineExercisesItemsViewModel);
            }
            foreach (var day in selectRoutineDaysMuscleGroupViewModel.DayGroupList)
            {
                _dayGroupListItemViewModels.Add(day);
            }
            _licenseDataStore = licenseDataStore;
            if (_licenseDataStore.CurrentGymProfile != null)
            {
                GymName = _licenseDataStore.CurrentGymProfile!.GymName;
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(_licenseDataStore.CurrentGymProfile!.Logo!);
                    bitmap.EndInit();
                    GymLogo = bitmap;
                }
                catch
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri("pack://application:,,,/Resources/Assets/logo.png");
                    bitmap.EndInit();
                    GymLogo = bitmap;
                }

            }
            else
            {

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri("pack://application:,,,/Resources/Assets/logo.png");
                bitmap.EndInit();
                GymLogo = bitmap;

            }

        }
        private string? _gymName;
        public string? GymName
        {
            get { return _gymName; }
            set { _gymName = value; OnPropertyChanged(nameof(GymName)); }
        }
        private BitmapImage? _gymLogo;

        public BitmapImage? GymLogo
        {
            get { return _gymLogo; }
            set { _gymLogo = value; OnPropertyChanged(nameof(GymLogo)); }
        }
        private void _routineDataStore_daysItemDeleted(DayGroupListItemViewModel obj)
        {
            _dayGroupListItemViewModels.Remove(obj);
        }

        private void _routineDataStore_daysItemCreated(DayGroupListItemViewModel obj)
        {
            _dayGroupListItemViewModels.Add(obj);
        }

        private readonly ObservableCollection<RoutineExercisesItemsViewModel> _routineExercisesItemsViewModels;

        private readonly ObservableCollection<DayGroupListItemViewModel> _dayGroupListItemViewModels;
        public IEnumerable<DayGroupListItemViewModel> DaysGroup => _dayGroupListItemViewModels;
        public IEnumerable<RoutineExercisesItemsViewModel> LegsExercisesList
            => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Legs).OrderBy(x => x.ItemOrder);
        public IEnumerable<RoutineExercisesItemsViewModel> ChestExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Chest).OrderBy(x => x.ItemOrder);
        public IEnumerable<RoutineExercisesItemsViewModel> BackExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Back).OrderBy(x => x.ItemOrder);
        public IEnumerable<RoutineExercisesItemsViewModel> ShouldersExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Shoulders).OrderBy(x => x.ItemOrder);
        public IEnumerable<RoutineExercisesItemsViewModel> BicepsExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Biceps).OrderBy(x => x.ItemOrder);
        public IEnumerable<RoutineExercisesItemsViewModel> TricepsExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Triceps).OrderBy(x => x.ItemOrder);
        public IEnumerable<RoutineExercisesItemsViewModel> CalvesExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Calves).OrderBy(x => x.ItemOrder);
        public IEnumerable<RoutineExercisesItemsViewModel> AbsExercisesList
         => _routineExercisesItemsViewModels.Where(x => x.GroupId == (int)EMuscleGroup.Abs).OrderBy(x => x.ItemOrder);
        public string? Date => _selectRoutineDaysMuscleGroupViewModel.Date.ToShortDateString();
        public string? Id => _selectRoutineDaysMuscleGroupViewModel.Number;
        public string? FullName => _playersDataStore.SelectedPlayer!.FullName;

        public override void Dispose()
        {
            _routineDataStore.daysItemCreated -= _routineDataStore_daysItemCreated;
            _routineDataStore.daysItemDeleted -= _routineDataStore_daysItemDeleted;
            base.Dispose();
        }
    }
}
