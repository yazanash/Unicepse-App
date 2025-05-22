using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.navigation.Stores;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;
using Uniceps.Commands.Player;
using Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands;
using Uniceps.navigation;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class RoutineItemListViewModel : ListingViewModelBase
    {
        private readonly RoutineItemDataStore _routineItemDataStore;
        private readonly NavigationStore _navigationStore;
        private readonly ExercisesViewModel _exercisesViewModel;
        private readonly DayGroupDataStore _dayGroupDataStore;
        private readonly ObservableCollection<RoutineItemListItemViewModel> _routineItemListItemViewModels;
        public IEnumerable<RoutineItemListItemViewModel> RoutineItems => _routineItemListItemViewModels;
        public ICommand OpenExercisesCommand { get; }
        public ICommand LoadRoutineItemCommand { get; }
        public RoutineItemListViewModel(RoutineItemDataStore routineItemDataStore, ExercisesViewModel exercisesViewModel, NavigationStore navigationStore, DayGroupDataStore dayGroupDataStore)
        {
            _routineItemDataStore = routineItemDataStore;
            _exercisesViewModel = exercisesViewModel;
            _navigationStore = navigationStore;
            _dayGroupDataStore = dayGroupDataStore;

            OpenExercisesCommand = new NavaigateCommand<ExercisesViewModel>(new NavigationService<ExercisesViewModel>(_navigationStore, () => _exercisesViewModel));
            _routineItemListItemViewModels = new ObservableCollection<RoutineItemListItemViewModel>();
            _dayGroupDataStore.DayGroupChanged += _dayGroupDataStore_DayGroupChanged;
            _routineItemDataStore.Loaded += _routineItemDataStore_Loaded;
            _routineItemDataStore.Created += _routineItemDataStore_Created;
            _routineItemDataStore.Updated += _routineItemDataStore_Updated;
            _routineItemDataStore.Deleted += _routineItemDataStore_Deleted;
            LoadRoutineItemCommand = new LoadRoutineItemsModelCommand(_dayGroupDataStore, _routineItemDataStore);
        }
        public RoutineItemListItemViewModel? SelectedRoutineItem
        {
            get
            {
                return RoutineItems
                    .FirstOrDefault(y => y?.RoutineItemModel == _routineItemDataStore.SelectedRoutineItem);
            }
            set
            {
                _routineItemDataStore.SelectedRoutineItem = value?.RoutineItemModel;
                OnPropertyChanged(nameof(SelectedRoutineItem));
            }
        }
        private void _dayGroupDataStore_DayGroupChanged(DayGroup? obj)
        {
            LoadRoutineItemCommand.Execute(null);
        }

        private void _routineItemDataStore_Deleted(int id)
        {
            RoutineItemListItemViewModel? itemViewModel = _routineItemListItemViewModels.FirstOrDefault(y => y.RoutineItemModel?.Id == id);

            if (itemViewModel != null)
            {
                _routineItemListItemViewModels.Remove(itemViewModel);
            }
        }

        private void _routineItemDataStore_Updated(RoutineItemModel obj)
        {
            RoutineItemListItemViewModel? viewModel =
                  _routineItemListItemViewModels.FirstOrDefault(y => y.RoutineItemModel.Id == obj.Id);

            if (viewModel != null)
            {
                viewModel.Update(obj);
            }
        }

        private void _routineItemDataStore_Created(RoutineItemModel obj)
        {
            AddRoutineItem(obj);
        }

        private void _routineItemDataStore_Loaded()
        {
            _routineItemListItemViewModels.Clear();
            foreach (RoutineItemModel routineItemModel in _routineItemDataStore.RoutineItems)
            {
                AddRoutineItem(routineItemModel);
            }
        }
        protected void AddRoutineItem(RoutineItemModel routineItemModel)
        {
            RoutineItemListItemViewModel viewModel = new RoutineItemListItemViewModel(routineItemModel, _routineItemDataStore);
            _routineItemListItemViewModels.Add(viewModel);
        }
    }
}
