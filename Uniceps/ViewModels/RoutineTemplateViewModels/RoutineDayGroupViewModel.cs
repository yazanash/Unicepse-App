using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.RoutineSystemCommands.DayGroupCommands;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class RoutineDayGroupViewModel : ViewModelBase
    {
        private readonly DayGroupDataStore _dayGroupDataStore;
        private readonly RoutineTempDataStore _routineTempDataStore;
        private readonly ObservableCollection<DayGroupViewModel> _dayGroupViewModels;
        public IEnumerable<DayGroupViewModel> DayGroups => _dayGroupViewModels;
        private bool _hasOrderChanged;
        public bool HasOrderChanged
        {
            get => _hasOrderChanged;
            set { _hasOrderChanged = value; OnPropertyChanged(nameof(HasOrderChanged)); }
        }
        public ICommand AddDayGroupCommand { get; }
        public ICommand LoadDayGroupCommand { get; }
        public ICommand SaveNewOrderCommand { get; }
        public ICommand MoveUpCommand { get; }
        public ICommand MoveDownCommand { get; }
        public bool HasData => _dayGroupViewModels.Count > 0;
        public RoutineDayGroupViewModel(DayGroupDataStore dayGroupDataStore, RoutineTempDataStore routineTempDataStore)
        {
            _dayGroupDataStore = dayGroupDataStore;
            _dayGroupViewModels = new();
            _routineTempDataStore = routineTempDataStore;
            SaveNewOrderCommand = new SaveReorderCommand(_dayGroupDataStore, this);
            AddDayGroupCommand = new CreateDayGroupCommand(_dayGroupDataStore, _routineTempDataStore);
            LoadDayGroupCommand = new LoadDayGroupsCommand(_dayGroupDataStore, _routineTempDataStore);
            _routineTempDataStore.RoutineChanged += _routineTempDataStore_RoutineChanged;
            _dayGroupDataStore.Loaded += _dayGroupDataStore_Loaded;
            _dayGroupDataStore.Created += _dayGroupDataStore_Created;
            _dayGroupDataStore.Updated += _dayGroupDataStore_Updated;
            _dayGroupDataStore.Deleted += _dayGroupDataStore_Deleted;
            MoveUpCommand = new RelayCommand(MoveUp);
            MoveDownCommand = new RelayCommand(MoveDown);
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
        private void MoveUp()
        {
            if(SelectedDayGroup!=null)
            {
                int index = _dayGroupViewModels.IndexOf(SelectedDayGroup);
                if (index <= 0) return;

                _dayGroupViewModels.Move(index, index - 1);
                OnDayGroupReordered();
            }
           
        }

        private void MoveDown()
        {
            if (SelectedDayGroup != null)
            {
                int index = _dayGroupViewModels.IndexOf(SelectedDayGroup);
                if (index >= _dayGroupViewModels.Count - 1) return;

                _dayGroupViewModels.Move(index, index + 1);
                OnDayGroupReordered();
            }
        }
        public void OnDayGroupReordered()
        {
            HasOrderChanged = true;
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
                    SaveNewOrderCommand.Execute(null);
                }
                else if (_dayGroupViewModels.Count > 0)
                {
                    _dayGroupDataStore.SelectedDayGroup = _dayGroupViewModels[0].DayGroup;
                    SaveNewOrderCommand.Execute(null);
                }
                else
                {
                    _dayGroupDataStore.SelectedDayGroup = null;
                }
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _dayGroupDataStore_Updated(DayGroup obj)
        {
            DayGroupViewModel? viewModel =
                _dayGroupViewModels.FirstOrDefault(y => y.DayGroup!.Id == obj.Id);

            if (viewModel != null)
            {
                viewModel.Update(obj);
            }
            OnPropertyChanged(nameof(HasData));
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
            SelectedDayGroup = _dayGroupViewModels.FirstOrDefault();
        }
        protected void AddDayGroup(DayGroup dayGroup)
        {
            DayGroupViewModel viewModel = new DayGroupViewModel(dayGroup, _dayGroupDataStore);
            _dayGroupViewModels.Add(viewModel);
            OnPropertyChanged(nameof(HasData));
        }
    }
}
