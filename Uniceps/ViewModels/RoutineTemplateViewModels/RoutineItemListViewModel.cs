using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Commands.RoutineSystemCommands.DayGroupCommands;
using Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Views.RoutineTemplateViews.RoutineComponent;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class RoutineItemListViewModel : ListingViewModelBase 
    {
        private readonly RoutineItemDataStore _routineItemDataStore;
        private readonly NavigationStore _navigationStore;
        private readonly ExercisesListViewModel _exercisesViewModel;
        private readonly DayGroupDataStore _dayGroupDataStore;
        private readonly SetsModelDataStore _setsModelDataStore;
        private readonly ObservableCollection<RoutineItemListItemViewModel> _routineItemListItemViewModels;
        public IEnumerable<RoutineItemListItemViewModel> RoutineItems => _routineItemListItemViewModels;
        public ICommand OpenExercisesCommand { get; }
        public ICommand LoadRoutineItemCommand { get; }
        public bool HasData => _routineItemListItemViewModels.Count > 0;
        private bool _hasOrderChanged;
        public bool HasOrderChanged
        {
            get => _hasOrderChanged;
            set { _hasOrderChanged = value; OnPropertyChanged(nameof(HasOrderChanged)); }
        }
        public ICommand SaveNewOrderCommand { get; }
        public ICommand MoveUpCommand { get; }
        public ICommand MoveDownCommand { get; }
        public ICommand AddSetsCommand { get; }
        public ICommand ApplySetsToAllCommand { get; }

        public RoutineItemListViewModel(RoutineItemDataStore routineItemDataStore, ExercisesListViewModel exercisesViewModel, NavigationStore navigationStore, DayGroupDataStore dayGroupDataStore, SetsModelDataStore setsModelDataStore)
        {
            _routineItemDataStore = routineItemDataStore;
            _exercisesViewModel = exercisesViewModel;
            _navigationStore = navigationStore;
            _dayGroupDataStore = dayGroupDataStore;
            SaveNewOrderCommand = new SaveRoutineItemReorderCommand(_routineItemDataStore, this);
            OpenExercisesCommand = new RelayCommand(ExecuteOpenExercisesCommand);
            _routineItemListItemViewModels = new ObservableCollection<RoutineItemListItemViewModel>();
            _dayGroupDataStore.DayGroupChanged += _dayGroupDataStore_DayGroupChanged;
            _routineItemDataStore.Loaded += _routineItemDataStore_Loaded;
            _routineItemDataStore.Created += _routineItemDataStore_Created;
            _routineItemDataStore.Updated += _routineItemDataStore_Updated;
            _routineItemDataStore.Deleted += _routineItemDataStore_Deleted;
            _dayGroupDataStore.Deleted += _dayGroupDataStore_Deleted;
            LoadRoutineItemCommand = new LoadRoutineItemsModelCommand(_dayGroupDataStore, _routineItemDataStore);
            _setsModelDataStore = setsModelDataStore;
            _setsModelDataStore.Created += _setsModelDataStore_Created;
            _setsModelDataStore.Updated += _setsModelDataStore_Updated;
            _setsModelDataStore.AppliedToAll += _setsModelDataStore_AppliedToAll;
            _setsModelDataStore.DeletedSet += _setsModelDataStore_DeletedSet; ;
            AddSetsCommand = new RelayCommand<RoutineItemListItemViewModel>(ExecuteAddSetsCommand);
            MoveUpCommand = new RelayCommand(MoveUp);
            MoveDownCommand = new RelayCommand(MoveDown);
            ApplySetsToAllCommand = new ApplySetsToAllCommand(_setsModelDataStore, this);

        }

        private void _dayGroupDataStore_Deleted(int obj)
        {
           if(_routineItemListItemViewModels.Any(x=>x.RoutineItemModel.DayId == obj))
            {
                _routineItemListItemViewModels.Clear();
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _setsModelDataStore_DeletedSet(int setId, int itemId)
        {
            var item = _routineItemListItemViewModels.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
                item.RemoveSet(setId);
        }
        private void _setsModelDataStore_AppliedToAll(List<SetModel> obj,int id)
        {
            var item = _routineItemListItemViewModels.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                item.RoutineItemModel.Sets.Clear();
                foreach(var set in obj)
                {
                    item.UpdateSets(set);
                }
            }
              
        }

        private void _setsModelDataStore_Updated(SetModel obj)
        {
            var item = _routineItemListItemViewModels.FirstOrDefault(x=>x.Id == obj.RoutineItemId);
            if(item!=null)
            item.UpdateSets(obj);
        }

        private void _setsModelDataStore_Created(SetModel obj)
        {
            var item = _routineItemListItemViewModels.FirstOrDefault(x => x.Id == obj.RoutineItemId);
            if (item != null)
                item.UpdateSets(obj);     
        }

        public void ExecuteOpenExercisesCommand()
        {
            if (_dayGroupDataStore.SelectedDayGroup != null)
            {
                ExercisesListViewWindow exercisesListViewWindow = new ExercisesListViewWindow();
                _exercisesViewModel.ClearSelection();
                _exercisesViewModel.SetChecks();
                exercisesListViewWindow.DataContext = _exercisesViewModel;
                exercisesListViewWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("يرجى تحديد يوم");
            }

        }
        private void MoveUp()
        {
            if (SelectedRoutineItem != null)
            {
                int index = _routineItemListItemViewModels.IndexOf(SelectedRoutineItem);
                if (index <= 0) return;

                _routineItemListItemViewModels.Move(index, index - 1);
                OnDayGroupReordered();
            }

        }

        private void MoveDown()
        {
            if (SelectedRoutineItem != null)
            {
                int index = _routineItemListItemViewModels.IndexOf(SelectedRoutineItem);
                if (index >= _routineItemListItemViewModels.Count - 1) return;

                _routineItemListItemViewModels.Move(index, index + 1);
                OnDayGroupReordered();
            }
        }
        public void OnDayGroupReordered()
        {
            HasOrderChanged = true;
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
            SaveNewOrderCommand.Execute(null);
            OnPropertyChanged(nameof(HasData));
        }

        private void _routineItemDataStore_Updated(RoutineItemModel obj)
        {
            RoutineItemListItemViewModel? viewModel =
                  _routineItemListItemViewModels.FirstOrDefault(y => y.RoutineItemModel.Id == obj.Id);

            if (viewModel != null)
            {
                viewModel.Update(obj);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _routineItemDataStore_Created(RoutineItemModel obj)
        {
            AddRoutineItem(obj);
            SaveNewOrderCommand.Execute(null);
        }

        public void ExecuteAddSetsCommand(RoutineItemListItemViewModel routineItemListItemViewModel)
        {
            SetModelItemsListViewModel setModelItemsListViewModel = new SetModelItemsListViewModel(_routineItemDataStore, _setsModelDataStore, routineItemListItemViewModel);
            RoutineItemModelCardWindowView routineItemModelCardWindowView = new RoutineItemModelCardWindowView();
            routineItemModelCardWindowView.DataContext = setModelItemsListViewModel;
            routineItemModelCardWindowView.ShowDialog();
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
            OnPropertyChanged(nameof(HasData));
        }
      
    }
}
