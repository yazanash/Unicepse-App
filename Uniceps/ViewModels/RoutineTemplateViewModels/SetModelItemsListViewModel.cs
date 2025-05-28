using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.Player;
using Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands;
using Uniceps.navigation.Stores;
using Uniceps.navigation;
using Uniceps.Commands.RoutineSystemCommands.SetModelsCommands;
using Uniceps.ViewModels;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.Core.Models.RoutineModels;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class SetModelItemsListViewModel : ListingViewModelBase
    {
        private readonly RoutineItemDataStore _routineItemDataStore;

        private readonly SetsModelDataStore _setsModelDataStore;
        private readonly ObservableCollection<SetModelListItemViewModel> _setModelListItemViewModels;
        public IEnumerable<SetModelListItemViewModel> SetModelItems => _setModelListItemViewModels;
        public ICommand LoadSetsCommand { get; }
        public ICommand AddSetModelCommand { get; }
        public ICommand SaveNewOrderCommand { get; }

        private SetModelListItemViewModel? _incomingSetModelItemViewModel;
        public SetModelListItemViewModel? IncomingSetModelItemViewModel
        {
            get
            {
                return _incomingSetModelItemViewModel;
            }
            set
            {
                _incomingSetModelItemViewModel = value;
                OnPropertyChanged(nameof(IncomingSetModelItemViewModel));
            }
        }

        private SetModelListItemViewModel? _removedSetModelItemViewModel;
        public SetModelListItemViewModel? RemovedSetModelItemViewModel
        {
            get
            {
                return _removedSetModelItemViewModel;
            }
            set
            {
                _removedSetModelItemViewModel = value;
                OnPropertyChanged(nameof(RemovedSetModelItemViewModel));
            }
        }

        private SetModelListItemViewModel? _insertedSetModelItemViewModel;
        public SetModelListItemViewModel? InsertedSetModelItemViewModel
        {
            get
            {
                return _insertedSetModelItemViewModel;
            }
            set
            {
                _insertedSetModelItemViewModel = value;
                OnPropertyChanged(nameof(InsertedSetModelItemViewModel));
            }
        }

        private SetModelListItemViewModel? _targetSetModelItemViewModel;
        public SetModelListItemViewModel? TargetSetModelItemViewModel
        {
            get
            {
                return _targetSetModelItemViewModel;
            }
            set
            {
                _targetSetModelItemViewModel = value;
                OnPropertyChanged(nameof(TargetSetModelItemViewModel));
            }
        }

        public ICommand SetModelItemReceivedCommand { get; }
        public ICommand SetModelItemRemovedCommand { get; }
        public ICommand SetModelItemInsertedCommand { get; }
        public SetModelItemsListViewModel(RoutineItemDataStore routineItemDataStore, SetsModelDataStore setsModelDataStore)
        {
            _routineItemDataStore = routineItemDataStore;
            _setsModelDataStore = setsModelDataStore;
            SetModelItemReceivedCommand = new SetModelReceivedCommand(this);
            SetModelItemRemovedCommand = new SetModelRemovedCommand(this);
            SetModelItemInsertedCommand = new SetModelInsertedCommand(this);
            _setModelListItemViewModels = new ObservableCollection<SetModelListItemViewModel>();
            SaveNewOrderCommand = new SaveSetModelItemReorderCommand(_setsModelDataStore, this);
            LoadSetsCommand = new LoadSetModelsCommand(_setsModelDataStore, _routineItemDataStore);
            AddSetModelCommand = new AddSetModelCommand(_setsModelDataStore, _routineItemDataStore);
            _setsModelDataStore.Loaded += _setsModelDataStore_Loaded;
            _setsModelDataStore.Created += _setsModelDataStore_Created;
            _setsModelDataStore.Deleted += _setsModelDataStore_Deleted;
            _setsModelDataStore.Updated += _setsModelDataStore_Updated;
            _routineItemDataStore.RoutineItemChanged += _routineItemDataStore_RoutineItemChanged;
        }
        private string? _exerciseImage;
        public string? ExerciseImage
        {
            get { return _exerciseImage; }
            set { _exerciseImage = value; OnPropertyChanged(nameof(ExerciseImage)); }
        }

        private string? _exerciseName;
        public string? ExerciseName
        {
            get { return _exerciseName; }
            set { _exerciseName = value; OnPropertyChanged(nameof(ExerciseName)); }
        }
        private void _routineItemDataStore_RoutineItemChanged(RoutineItemModel? obj)
        {
            LoadSetsCommand.Execute(null);
            if (obj != null && obj.Exercise != null)
            {
                ExerciseImage = obj!.Exercise!.ImagePath;
                ExerciseName = obj!.Exercise!.Name;
            }

        }

        private void _setsModelDataStore_Updated(SetModel obj)
        {
            SetModelListItemViewModel? viewModel =
                  _setModelListItemViewModels.FirstOrDefault(y => y.SetModel!.Id == obj.Id);

            if (viewModel != null)
            {
                viewModel.Update(obj);
            }
        }

        private void _setsModelDataStore_Deleted(int obj)
        {
            SetModelListItemViewModel? itemViewModel = _setModelListItemViewModels.FirstOrDefault(y => y.SetModel?.Id == obj);

            if (itemViewModel != null)
            {
                _setModelListItemViewModels.Remove(itemViewModel);
            }
        }

        private void _setsModelDataStore_Created(SetModel obj)
        {
            AddSetModelItem(obj);
        }

        private void _setsModelDataStore_Loaded()
        {
            _setModelListItemViewModels.Clear();
            foreach (SetModel setModel in _setsModelDataStore.SetModels)
            {
                AddSetModelItem(setModel);
            }
        }



        protected void AddSetModelItem(SetModel setModel)
        {
            SetModelListItemViewModel viewModel = new SetModelListItemViewModel(setModel, _setsModelDataStore);
            _setModelListItemViewModels.Add(viewModel);
        }
        public void AddTodoItem(SetModelListItemViewModel item)
        {
            if (!_setModelListItemViewModels.Contains(item))
            {
                _setModelListItemViewModels.Add(item);
            }
        }

        public void InsertTodoItem(SetModelListItemViewModel insertedTodoItem, SetModelListItemViewModel targetTodoItem)
        {
            if (insertedTodoItem == targetTodoItem)
            {
                return;
            }

            int oldIndex = _setModelListItemViewModels.IndexOf(insertedTodoItem);
            int nextIndex = _setModelListItemViewModels.IndexOf(targetTodoItem);

            if (oldIndex != -1 && nextIndex != -1)
            {
               
                _setModelListItemViewModels.Move(oldIndex, nextIndex);
            }
        }

        public void RemoveTodoItem(SetModelListItemViewModel item)
        {
            _setModelListItemViewModels.Remove(item);
        }
    }
}
