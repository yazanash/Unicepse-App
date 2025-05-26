using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.utlis.common;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;
using Uniceps.Commands.RoutineSystemCommands.DayGroupCommands;
using System.Windows.Controls;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class RoutineDayGroupViewModel : ViewModelBase
    {
        private readonly DayGroupDataStore _dayGroupDataStore;
        private readonly RoutineTempDataStore _routineTempDataStore;
        private readonly ObservableCollection<DayGroupViewModel> _dayGroupViewModels;
        public IEnumerable<DayGroupViewModel> DayGroups => _dayGroupViewModels;
        public ICommand AddDayGroupCommand { get; }
        public ICommand LoadDayGroupCommand { get; }
        public ICommand SaveNewOrderCommand { get; }
        private DayGroupViewModel _incomingDayGroupViewModel;
        public DayGroupViewModel IncomingDayGroupViewModel
        {
            get
            {
                return _incomingDayGroupViewModel;
            }
            set
            {
                _incomingDayGroupViewModel = value;
                OnPropertyChanged(nameof(IncomingDayGroupViewModel));
            }
        }

        private DayGroupViewModel _removedDayGroupViewModel;
        public DayGroupViewModel RemovedDayGroupViewModel
        {
            get
            {
                return _removedDayGroupViewModel;
            }
            set
            {
                _removedDayGroupViewModel = value;
                OnPropertyChanged(nameof(RemovedDayGroupViewModel));
            }
        }

        private DayGroupViewModel _insertedDayGroupViewModel;
        public DayGroupViewModel InsertedDayGroupViewModel
        {
            get
            {
                return _insertedDayGroupViewModel;
            }
            set
            {
                _insertedDayGroupViewModel = value;
                OnPropertyChanged(nameof(InsertedDayGroupViewModel));
            }
        }

        private DayGroupViewModel _targetDayGroupViewModel;
        public DayGroupViewModel TargetDayGroupViewModel
        {
            get
            {
                return _targetDayGroupViewModel;
            }
            set
            {
                _targetDayGroupViewModel = value;
                OnPropertyChanged(nameof(TargetDayGroupViewModel));
            }
        }

        public ICommand DayGroupReceivedCommand { get; }
        public ICommand DayGroupRemovedCommand { get; }
        public ICommand DayGroupInsertedCommand { get; }

        public RoutineDayGroupViewModel(DayGroupDataStore dayGroupDataStore, RoutineTempDataStore routineTempDataStore)
        {
            _dayGroupDataStore = dayGroupDataStore;
            _dayGroupViewModels = new();
            _routineTempDataStore = routineTempDataStore;
            DayGroupReceivedCommand = new DayGroupReceivedCommand(this);
            DayGroupRemovedCommand = new DayGroupRemovedCommand(this);
            DayGroupInsertedCommand = new DayGroupInsertedCommand(this);
            SaveNewOrderCommand = new SaveReorderCommand(_dayGroupDataStore, this);
            AddDayGroupCommand = new CreateDayGroupCommand(_dayGroupDataStore, _routineTempDataStore);
            LoadDayGroupCommand = new LoadDayGroupsCommand(_dayGroupDataStore, _routineTempDataStore);
            _routineTempDataStore.RoutineChanged += _routineTempDataStore_RoutineChanged;
            _dayGroupDataStore.Loaded += _dayGroupDataStore_Loaded;
            _dayGroupDataStore.Created += _dayGroupDataStore_Created;
            _dayGroupDataStore.Updated += _dayGroupDataStore_Updated;
            _dayGroupDataStore.Deleted += _dayGroupDataStore_Deleted;
        }
       
        public DayGroupViewModel? SelectedDayGroup
        {
            get
            {
                return DayGroups
                    .FirstOrDefault(y => y?.DayGroup == _dayGroupDataStore.SelectedDayGroup);
            }
            set
            {
                _dayGroupDataStore.SelectedDayGroup = value?.DayGroup;
                OnPropertyChanged(nameof(SelectedDayGroup));
            }
        }
        private void _routineTempDataStore_RoutineChanged(RoutineModel? obj)
        {
            LoadDayGroupCommand.Execute(null);
        }
        private void _dayGroupDataStore_Deleted(int obj)
        {
            DayGroupViewModel? itemViewModel = _dayGroupViewModels.FirstOrDefault(y => y.DayGroup?.Id == obj);

            if (itemViewModel != null)
            {
                int index = _dayGroupViewModels.IndexOf(itemViewModel);
                _dayGroupViewModels.Remove(itemViewModel);
                if (index > 0)
                {

                    _dayGroupDataStore.SelectedDayGroup = _dayGroupViewModels[index - 1].DayGroup;
                }
                else if (_dayGroupViewModels.Count > 0)
                {
                    _dayGroupDataStore.SelectedDayGroup = _dayGroupViewModels[0].DayGroup;
                }

            }
        }

        private void _dayGroupDataStore_Updated(DayGroup obj)
        {
            DayGroupViewModel? viewModel =
                _dayGroupViewModels.FirstOrDefault(y => y.DayGroup!.Id == obj.Id);

            if (viewModel != null)
            {
                viewModel.Update(obj);
            }
        }

        private void _dayGroupDataStore_Created(DayGroup obj)
        {
            AddDayGroup(obj);
        }

        private void _dayGroupDataStore_Loaded()
        {
            _dayGroupViewModels.Clear();
            foreach (DayGroup dayGroup in _dayGroupDataStore.DayGroups.OrderBy(x => x.Order))
            {
                AddDayGroup(dayGroup);
            }
        }
        protected void AddDayGroup(DayGroup dayGroup)
        {
            DayGroupViewModel viewModel = new DayGroupViewModel(dayGroup, _dayGroupDataStore);
            _dayGroupViewModels.Add(viewModel);
        }
        public void AddTodoItem(DayGroupViewModel item)
        {
            if (!_dayGroupViewModels.Contains(item))
            {
                _dayGroupViewModels.Add(item);
            }
        }

        public void InsertTodoItem(DayGroupViewModel insertedTodoItem, DayGroupViewModel targetTodoItem)
        {
            if (insertedTodoItem == targetTodoItem)
            {
                return;
            }

            int oldIndex = _dayGroupViewModels.IndexOf(insertedTodoItem);
            int nextIndex = _dayGroupViewModels.IndexOf(targetTodoItem);

            if (oldIndex != -1 && nextIndex != -1)
            {
                _dayGroupViewModels.Move(oldIndex, nextIndex);
            }
        }

        public void RemoveTodoItem(DayGroupViewModel item)
        {
            _dayGroupViewModels.Remove(item);
        }
    }
}
