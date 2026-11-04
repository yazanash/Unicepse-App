using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.RoutineSystemCommands.RoutineModelCommands;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.navigation.DialogService;
using Uniceps.navigation.NavigationSystem;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;
using Uniceps.Views.RoutineTemplateViews;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class RoutineListViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<RoutineTemplateListItemViewModel> _routineTemplateListItemViewModels;
        private readonly RoutineTempDataStore _routineTempDataStore;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
      public ICollectionView RoutineList { get; set; }
        public ICommand LoadAllRoutines { get; }
        public ICommand AddRoutineCommand { get; }
        public SearchBoxViewModel SearchBox { get; set; }
        public ObservableCollection<RoutineLevel> RoutineLevels { get; set; } = new();
        public bool HasData => _routineTemplateListItemViewModels.Count > 0;
        public RoutineListViewModel(RoutineTempDataStore routineTempDataStore, INavigationService navigationService, IDialogService dialogService)
        {
            _routineTempDataStore = routineTempDataStore;
            _navigationService = navigationService;
            _dialogService = dialogService;
            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;
            _routineTemplateListItemViewModels = new ObservableCollection<RoutineTemplateListItemViewModel>();
            RoutineList = CollectionViewSource.GetDefaultView(_routineTemplateListItemViewModels);
            RoutineList.Filter = CheckRoutineFilter;
            AddRoutineCommand = new RelayCommand(()=> _dialogService.ShowDialog<CreateRoutineWindowView, CreateRoutineViewModel>());
            LoadAllRoutines = new LoadRoutineModelsCommand(_routineTempDataStore);
            _routineTempDataStore.Loaded += _routineTempDataStore_Loaded;
            _routineTempDataStore.Created += _routineTempDataStore_Created;
            _routineTempDataStore.Updated += _routineTempDataStore_Updated;
            _routineTempDataStore.Deleted += _routineTempDataStore_Deleted;
            foreach (var item in Enum.GetValues(typeof(RoutineLevel)))
            {
                RoutineLevels.Add((RoutineLevel)item);
            }
            LoadAllRoutines.Execute(null);
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
            RoutineTemplateListItemViewModel viewModel = new RoutineTemplateListItemViewModel(routineModel, _navigationService);
            _routineTemplateListItemViewModels.Add(viewModel);
            viewModel.Order = _routineTemplateListItemViewModels.Count;
            OnPropertyChanged(nameof(HasData));
        }
    }
}
