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
using Uniceps.Commands.RoutineSystemCommands.DayGroupCommands;

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

        public ICommand SaveNewOrderCommand { get; }

        private RoutineItemListItemViewModel _incomingRoutineItemViewModel;
        public RoutineItemListItemViewModel IncomingRoutineItemViewModel
        {
            get
            {
                return _incomingRoutineItemViewModel;
            }
            set
            {
                _incomingRoutineItemViewModel = value;
                OnPropertyChanged(nameof(IncomingRoutineItemViewModel));
            }
        }

        private RoutineItemListItemViewModel _removedRoutineItemViewModel;
        public RoutineItemListItemViewModel RemovedRoutineItemViewModel
        {
            get
            {
                return _removedRoutineItemViewModel;
            }
            set
            {
                _removedRoutineItemViewModel = value;
                OnPropertyChanged(nameof(RemovedRoutineItemViewModel));
            }
        }

        private RoutineItemListItemViewModel _insertedRoutineItemViewModel;
        public RoutineItemListItemViewModel InsertedRoutineItemViewModel
        {
            get
            {
                return _insertedRoutineItemViewModel;
            }
            set
            {
                _insertedRoutineItemViewModel = value;
                OnPropertyChanged(nameof(InsertedRoutineItemViewModel));
            }
        }

        private RoutineItemListItemViewModel _targetRoutineItemViewModel;
        public RoutineItemListItemViewModel TargetRoutineItemViewModel
        {
            get
            {
                return _targetRoutineItemViewModel;
            }
            set
            {
                _targetRoutineItemViewModel = value;
                OnPropertyChanged(nameof(TargetRoutineItemViewModel));
            }
        }

        public ICommand RoutineItemReceivedCommand { get; }
        public ICommand RoutineItemRemovedCommand { get; }
        public ICommand RoutineItemInsertedCommand { get; }
        public RoutineItemListViewModel(RoutineItemDataStore routineItemDataStore, ExercisesViewModel exercisesViewModel, NavigationStore navigationStore, DayGroupDataStore dayGroupDataStore)
        {
            _routineItemDataStore = routineItemDataStore;
            _exercisesViewModel = exercisesViewModel;
            _navigationStore = navigationStore;
            _dayGroupDataStore = dayGroupDataStore;
            RoutineItemReceivedCommand = new RoutineItemReceivedCommand(this);
            RoutineItemRemovedCommand = new RoutineItemRemovedCommand(this);
            RoutineItemInsertedCommand = new RoutineItemInsertedCommand(this);
            SaveNewOrderCommand = new SaveRoutineItemReorderCommand(_routineItemDataStore, this);
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
            foreach (RoutineItemModel routineItemModel in _routineItemDataStore.RoutineItems.OrderBy(x=>x.Order))
            {
                AddRoutineItem(routineItemModel);
            }
        }
        protected void AddRoutineItem(RoutineItemModel routineItemModel)
        {
            RoutineItemListItemViewModel viewModel = new RoutineItemListItemViewModel(routineItemModel, _routineItemDataStore);
            _routineItemListItemViewModels.Add(viewModel);
        }
        public void AddTodoItem(RoutineItemListItemViewModel item)
        {
            if (!_routineItemListItemViewModels.Contains(item))
            {
                _routineItemListItemViewModels.Add(item);
            }
        }

        public void InsertTodoItem(RoutineItemListItemViewModel insertedTodoItem, RoutineItemListItemViewModel targetTodoItem)
        {
            if (insertedTodoItem == targetTodoItem)
            {
                return;
            }

            int oldIndex = _routineItemListItemViewModels.IndexOf(insertedTodoItem);
            int nextIndex = _routineItemListItemViewModels.IndexOf(targetTodoItem);

            if (oldIndex != -1 && nextIndex != -1)
            {
                _routineItemListItemViewModels.Move(oldIndex, nextIndex);
            }
        }

        public void RemoveTodoItem(RoutineItemListItemViewModel item)
        {
            _routineItemListItemViewModels.Remove(item);
        }
    }
}
