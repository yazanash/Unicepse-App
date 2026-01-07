using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Commands.RoutineSystemCommands.RoutineModelCommands;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores.RoutineStores;
using Uniceps.utlis.common;
using Uniceps.ViewModels;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;
using Uniceps.ViewModels.SportsViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Views.RoutineTemplateViews;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class RoutineListViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<RoutineTemplateListItemViewModel> _routineTemplateListItemViewModels;
        private readonly RoutineTempDataStore _routineTempDataStore;
        private readonly NavigationStore _navigatorStore;
        private readonly RoutineDetailsViewModel _routineDetailsViewModel;
        public ICollectionView RoutineList { get; set; }
        public ICommand LoadAllRoutines { get; }
        public ICommand AddRoutineCommand { get; }
        public SearchBoxViewModel SearchBox { get; set; }
        public ObservableCollection<RoutineLevel> RoutineLevels { get; set; } = new();
        public bool HasData => _routineTemplateListItemViewModels.Count > 0;
        public RoutineListViewModel(RoutineTempDataStore routineTempDataStore, NavigationStore navigatorStore, RoutineDetailsViewModel routineDetailsViewModel)
        {
            _routineTempDataStore = routineTempDataStore;
            _navigatorStore = navigatorStore;
            _routineDetailsViewModel = routineDetailsViewModel;
            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;
            _routineTemplateListItemViewModels = new ObservableCollection<RoutineTemplateListItemViewModel>();
            RoutineList = CollectionViewSource.GetDefaultView(_routineTemplateListItemViewModels);
            RoutineList.Filter = CheckRoutineFilter;
            AddRoutineCommand = new RelayCommand(ExecuteAddRoutineCommmand);
            LoadAllRoutines = new LoadRoutineModelsCommand(_routineTempDataStore);
            _routineTempDataStore.Loaded += _routineTempDataStore_Loaded;
            _routineTempDataStore.Created += _routineTempDataStore_Created;
            _routineTempDataStore.Updated += _routineTempDataStore_Updated;
            _routineTempDataStore.Deleted += _routineTempDataStore_Deleted;
            foreach (var item in Enum.GetValues(typeof(RoutineLevel)))
            {
                RoutineLevels.Add((RoutineLevel)item);
            }
        }

        private void SearchBox_SearchedText(string? obj)
        {
            RoutineFilter = obj;
            RoutineList.Refresh();
        }

        private bool CheckRoutineFilter(object obj)
        {
            if (obj is RoutineTemplateListItemViewModel routine)
            {
                bool matchText =
                    string.IsNullOrEmpty(RoutineFilter) ||
                    routine.Name!.Contains(RoutineFilter, StringComparison.OrdinalIgnoreCase);

                bool matchStatus = SelectedLevel == RoutineLevel.None || routine.Level == SelectedLevel;

                return matchText && matchStatus;
            }
            return false;
        }

        public void ExecuteAddRoutineCommmand()
        {
            CreateRoutineViewModel createRoutineViewModel =  new CreateRoutineViewModel(_routineTempDataStore);
            CreateRoutineWindowView createRoutineWindowView = new CreateRoutineWindowView();
            createRoutineWindowView.DataContext = createRoutineViewModel;
            createRoutineWindowView.ShowDialog();
        }
        private RoutineLevel _selectedLevel;
        private string? _routineFilter;
        public string? RoutineFilter
        {
            get { return _routineFilter; }
            set { _routineFilter = value; OnPropertyChanged(nameof(RoutineFilter)); }
        }

        public RoutineLevel SelectedLevel
        {
            get { return _selectedLevel; }
            set
            {
                _selectedLevel = value; OnPropertyChanged(nameof(SelectedLevel));
                RoutineList.Refresh();
            }
        }
        public RoutineTemplateListItemViewModel? SelectedRoutine
        {
            get
            {
                return _routineTemplateListItemViewModels
                    .FirstOrDefault(y => y?.RoutineModel == _routineTempDataStore.SelectedRoutine);
            }
            set
            {
                _routineTempDataStore.SelectedRoutine = value?.RoutineModel;

            }
        }
        private void _routineTempDataStore_Deleted(int id)
        {
            RoutineTemplateListItemViewModel? itemViewModel = _routineTemplateListItemViewModels.FirstOrDefault(y => y.RoutineModel?.Id == id);

            if (itemViewModel != null)
            {
                _routineTemplateListItemViewModels.Remove(itemViewModel);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _routineTempDataStore_Updated(RoutineModel obj)
        {
            RoutineTemplateListItemViewModel? viewModel =
                  _routineTemplateListItemViewModels.FirstOrDefault(y => y.RoutineModel.Id == obj.Id);

            if (viewModel != null)
            {
                viewModel.Update(obj);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _routineTempDataStore_Created(RoutineModel obj)
        {
            AddRoutine(obj);
            OnPropertyChanged(nameof(HasData));
        }

        private void _routineTempDataStore_Loaded()
        {
            _routineTemplateListItemViewModels.Clear();
            foreach (RoutineModel routineModel in _routineTempDataStore.Routines)
            {
                AddRoutine(routineModel);
            }
        }
        protected void AddRoutine(RoutineModel routineModel)
        {
            RoutineTemplateListItemViewModel viewModel = new RoutineTemplateListItemViewModel(routineModel, _navigatorStore, _routineDetailsViewModel);
            _routineTemplateListItemViewModels.Add(viewModel);
            viewModel.Order = _routineTemplateListItemViewModels.Count + 1;
            OnPropertyChanged(nameof(HasData));
        }
        public static RoutineListViewModel LoadViewModel(RoutineTempDataStore routineTempDataStore, NavigationStore navigatorStore, RoutineDetailsViewModel routineDetailsViewModel)
        {
            RoutineListViewModel routineListViewModel = new RoutineListViewModel(routineTempDataStore, navigatorStore, routineDetailsViewModel);
            routineListViewModel.LoadAllRoutines.Execute(null);
            return routineListViewModel;
        }
    }
}
