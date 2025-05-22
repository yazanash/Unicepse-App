using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.Player;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.Commands.RoutineSystemCommands.RoutineModelCommands;
using Uniceps.navigation;
using Uniceps.ViewModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.navigation.Stores;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class CreateRoutineViewModel : ErrorNotifyViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly RoutineTempDataStore _routineTempDataStore;
        public CreateRoutineViewModel(NavigationStore navigationStore, RoutineListViewModel routineListViewModel, RoutineTempDataStore routineTempDataStore)
        {
            _navigationStore = navigationStore;
            _routineTempDataStore = routineTempDataStore;
            NavigationService<RoutineListViewModel> navigationService = new NavigationService<RoutineListViewModel>(_navigationStore, () => routineListViewModel);
            CancelCommand = new NavaigateCommand<RoutineListViewModel>(navigationService);

            SubmitCommand = new CreateRoutineModelCommand(_routineTempDataStore, this, navigationService);
        }


        #region Properties
        public int Id { get; }

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
            }
        }
        private string? _level = "0";
        public string? Level
        {
            get { return _level; }
            set
            {
                _level = value; OnPropertyChanged(nameof(Level));
                ClearError(nameof(Level));
                if (Level?.Trim().Length < 10)
                {
                    AddError("هذا الحقل مطلوب", nameof(Level));
                    OnErrorChanged(nameof(Level));
                }

            }
        }
        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
        #endregion

    }
}
