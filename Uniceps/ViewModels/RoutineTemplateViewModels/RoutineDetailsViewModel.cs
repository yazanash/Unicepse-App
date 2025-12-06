using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.RoutineSystemCommands.RoutineModelCommands;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.navigation;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;
using Uniceps.Views.RoutineTemplateViews;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class RoutineDetailsViewModel : ErrorNotifyViewModelBase
    {
        public RoutineDayGroupViewModel RoutineDayGroupViewModel { get; set; }
        public RoutineItemListViewModel RoutineItemListViewModel { get; set; }
        private readonly RoutineTempDataStore _routineTempDataStore;
        private readonly NavigationService<RoutineListViewModel> _navigationService;
        public ObservableCollection<RoutineLevel> RoutineLevels { get; set; } = new();
        public RoutineDetailsViewModel(RoutineDayGroupViewModel routineDayGroupViewModel, RoutineItemListViewModel routineItemListViewModel, RoutineTempDataStore routineTempDataStore, NavigationService<RoutineListViewModel> navigationService)
        {
            RoutineDayGroupViewModel = routineDayGroupViewModel;
            RoutineItemListViewModel = routineItemListViewModel;
            _routineTempDataStore = routineTempDataStore;
            _navigationService = navigationService;
            _routineTempDataStore.RoutineChanged += _routineTempDataStore_RoutineChanged;
            BackCommand = new RelayCommand(ExecuteBackCommand);
            ExportCommand = new RelayCommand(ExecuteExportCommand);
            UpdateRoutineCommand = new UpdateRoutineModelCommand(_routineTempDataStore, this);
            DeleteRoutineCommand = new DeleteRoutineModelCommand(_routineTempDataStore, _navigationService);
            foreach (var item in Enum.GetValues(typeof(RoutineLevel)))
            {
                RoutineLevels.Add((RoutineLevel)item);
            }
        }

        private void _routineTempDataStore_RoutineChanged(RoutineModel? obj)
        {
            if (obj != null)
            {
                Name = obj.Name;
                Level = RoutineLevels.FirstOrDefault(x => x.Equals(obj.Level));
            }
        }
        private bool _isEditable = false;
        public bool IsEditable
        {
            get { return _isEditable; }
            set
            {
                _isEditable = value; OnPropertyChanged(nameof(Level));
                OnPropertyChanged(nameof(IsEditable));
            }
        }
        private void ExecuteBackCommand()
        {
            _navigationService.ReNavigate();
        }
        private void ExecuteExportCommand()
        {
            ExportDialogViewModel exportDialogViewModel = new ExportDialogViewModel(_routineTempDataStore);
            ExportDialogWindow exportDialogWindow = new ExportDialogWindow();
            exportDialogWindow.DataContext = exportDialogViewModel;
            exportDialogWindow.ShowDialog();
        }
        private string? _name;
        public string? Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                ClearError(nameof(Name));
                if (string.IsNullOrEmpty(Name?.Trim()))
                {
                    AddError("هذا الحقل مطلوب", nameof(Name));
                    OnErrorChanged(nameof(Name));
                }
                IsEditable = Name != _routineTempDataStore.SelectedRoutine?.Name;
                OnPropertyChanged(nameof(IsEditable));
            }
        }
        private RoutineLevel _level;
        public RoutineLevel Level
        {
            get { return _level; }
            set
            {
                _level = value; OnPropertyChanged(nameof(Level));
                IsEditable = Level != _routineTempDataStore.SelectedRoutine?.Level;
                OnPropertyChanged(nameof(IsEditable));
            }
        }
        public ICommand UpdateRoutineCommand { get; }
        public ICommand DeleteRoutineCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand ExportCommand { get; }

    }
}
