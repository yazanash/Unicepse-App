using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands;
using Uniceps.Commands.RoutineSystemCommands.SetModelsCommands;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class SetModelItemsListViewModel : ListingViewModelBase
    {
        private readonly RoutineItemDataStore _routineItemDataStore;

        private readonly SetsModelDataStore _setsModelDataStore;
        private readonly ObservableCollection<SetModelListItemViewModel> _setModelListItemViewModels;
        public IEnumerable<SetModelListItemViewModel> SetModelItems => _setModelListItemViewModels;
        private bool _hasOrderChanged;
        public bool HasOrderChanged
        {
            get => _hasOrderChanged;
            set { _hasOrderChanged = value; OnPropertyChanged(nameof(HasOrderChanged)); }
        }
        public bool HasData => _setModelListItemViewModels.Count > 0;
        public ICommand LoadSetsCommand { get; }
        public ICommand AddSetModelCommand { get; }
        public ICommand SaveNewOrderCommand { get; }
        public ICommand MoveUpCommand { get; }
        public ICommand MoveDownCommand { get; }
        public ICommand SaveAllCommand { get; }

        public SetModelItemsListViewModel(RoutineItemDataStore routineItemDataStore, SetsModelDataStore setsModelDataStore, RoutineItemListItemViewModel selectedRoutineItem)
        {
            _routineItemDataStore = routineItemDataStore;
            _setsModelDataStore = setsModelDataStore;
            SelectedRoutineItem = selectedRoutineItem;
            _setModelListItemViewModels = new ObservableCollection<SetModelListItemViewModel>();
            SaveNewOrderCommand = new SaveSetModelItemReorderCommand(_setsModelDataStore, this);
            LoadSetsCommand = new LoadSetModelsCommand(_setsModelDataStore, _routineItemDataStore);
            AddSetModelCommand = new AddSetModelCommand(_setsModelDataStore, SelectedRoutineItem);
            _setsModelDataStore.Loaded += _setsModelDataStore_Loaded;
            _setsModelDataStore.Created += _setsModelDataStore_Created;
            _setsModelDataStore.Deleted += _setsModelDataStore_Deleted;
            _setsModelDataStore.Updated += _setsModelDataStore_Updated;
            LoadSetsCommand.Execute(null);
            if (SelectedRoutineItem.RoutineItemModel != null && SelectedRoutineItem.RoutineItemModel.Exercise != null)
            {
                ExerciseImage = SelectedRoutineItem.RoutineItemModel!.Exercise!.ImagePath;
                ExerciseName = SelectedRoutineItem.RoutineItemModel!.Exercise!.Name;
            }
            MoveUpCommand = new RelayCommand(MoveUp);
            MoveDownCommand = new RelayCommand(MoveDown);
            SaveAllCommand = new RelayCommand(ExecuteSaveAllCommand);
        }
        public Action? SetsUpdated;
        public void OnSetsUpdated()
        {
            SetsUpdated?.Invoke();
        }
        private void ExecuteSaveAllCommand()
        {
            foreach(var item in _setModelListItemViewModels)
            {
                item.SubmitCommand.Execute(null);
            }
            OnSetsUpdated();
        }
        private void MoveUp()
        {
            if (SelectedSetModelItem != null)
            {
                int index = _setModelListItemViewModels.IndexOf(SelectedSetModelItem);
                if (index <= 0) return;

                _setModelListItemViewModels.Move(index, index - 1);
                OnDayGroupReordered();
            }

        }

        private void MoveDown()
        {
            if (SelectedSetModelItem != null)
            {
                int index = _setModelListItemViewModels.IndexOf(SelectedSetModelItem);
                if (index >= _setModelListItemViewModels.Count - 1) return;

                _setModelListItemViewModels.Move(index, index + 1);
                OnDayGroupReordered();
            }
        }
        public void OnDayGroupReordered()
        {
            HasOrderChanged = true;
        }
        private SetModelListItemViewModel? _selectedSetModelItem;
        public SetModelListItemViewModel? SelectedSetModelItem
        {
            get
            {
                return _selectedSetModelItem;
            }
            set
            {
                _selectedSetModelItem = value;
                OnPropertyChanged(nameof(SelectedSetModelItem));
            }
        }
        public RoutineItemListItemViewModel SelectedRoutineItem;
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
        private void _setsModelDataStore_Updated(SetModel obj)
        {
            SetModelListItemViewModel? viewModel =
                  _setModelListItemViewModels.FirstOrDefault(y => y.SetModel!.Id == obj.Id);

            if (viewModel != null)
            {
                viewModel.Update(obj);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _setsModelDataStore_Deleted(int obj)
        {
            SetModelListItemViewModel? itemViewModel = _setModelListItemViewModels.FirstOrDefault(y => y.SetModel?.Id == obj);

            if (itemViewModel != null)
            {
                _setModelListItemViewModels.Remove(itemViewModel);
            }
            SaveNewOrderCommand.Execute(null);
            OnPropertyChanged(nameof(HasData));
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
            OnPropertyChanged(nameof(HasData));
        }
    
    }
}
