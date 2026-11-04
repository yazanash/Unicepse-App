using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.RoutineSystemCommands.RoutineModelCommands;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.navigation.DialogService;
using Uniceps.navigation.Extensions;
using Uniceps.navigation.NavigationSystem;
using Uniceps.Stores.RoutineStores;
using Uniceps.Views.RoutineTemplateViews;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class RoutineDetailsViewModel : ErrorNotifyViewModelBase,IQueryAttributable
    {
        public RoutineDayGroupViewModel RoutineDayGroupViewModel { get; set; }
        public RoutineItemListViewModel RoutineItemListViewModel { get; set; }
        private readonly RoutineTempDataStore _routineTempDataStore;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        public ObservableCollection<RoutineLevel> RoutineLevels { get; set; } = new();
        public RoutineModel? RoutineModel { get; set; }
        public RoutineDetailsViewModel(RoutineDayGroupViewModel routineDayGroupViewModel, RoutineItemListViewModel routineItemListViewModel, RoutineTempDataStore routineTempDataStore, INavigationService navigationService, IDialogService dialogService)
        {
            RoutineDayGroupViewModel = routineDayGroupViewModel;
            RoutineItemListViewModel = routineItemListViewModel;
            _routineTempDataStore = routineTempDataStore;
            _navigationService = navigationService;
            _dialogService = dialogService;

            BackCommand = new RelayCommand(ExecuteBackCommand);
            ExportCommand = new RelayCommand(ExecuteExportCommand);
            UpdateRoutineCommand = new UpdateRoutineModelCommand(_routineTempDataStore, this);
            DeleteRoutineCommand = new DeleteRoutineModelCommand(_routineTempDataStore, _navigationService, this);
            foreach (var item in Enum.GetValues(typeof(RoutineLevel)))
            {
                RoutineLevels.Add((RoutineLevel)item);
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
            _navigationService.GoBack();
        }
        private void ExecuteExportCommand()
        {
            if (RoutineModel != null)
            {
                var navParams = new Dictionary<string, object>()
            {
                {"RoutineModel", RoutineModel }
            };
                _dialogService.ShowDialog<ExportDialogWindow, ExportDialogViewModel>(navParams);
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue<RoutineModel>("RoutineModel", out RoutineModel? routineModel))
            {
                if (routineModel != null)
                {
                    RoutineModel = routineModel;
                    Name = RoutineModel.Name;
                    Level = RoutineLevels.FirstOrDefault(x => x.Equals(RoutineModel.Level));
                    var navQuery = new Dictionary<string, object>()
                    {
                        {"RoutineId",RoutineModel.Id }
                    };
                    RoutineDayGroupViewModel.ApplyQueryAttributes(navQuery);
                }
            }

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
                IsEditable = Name != RoutineModel?.Name;
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
                IsEditable = Level != RoutineModel?.Level;
                OnPropertyChanged(nameof(IsEditable));
            }
        }
        public ICommand UpdateRoutineCommand { get; }
        public ICommand DeleteRoutineCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand ExportCommand { get; }

    }
}
