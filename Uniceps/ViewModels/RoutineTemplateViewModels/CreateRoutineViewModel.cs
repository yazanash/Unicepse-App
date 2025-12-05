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
using Uniceps.Core.Models.RoutineModels;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class CreateRoutineViewModel : ErrorNotifyViewModelBase
    {
        private readonly RoutineTempDataStore _routineTempDataStore;
        public ObservableCollection<RoutineLevel> RoutineLevels{ get; set; } = new();
        public CreateRoutineViewModel(RoutineTempDataStore routineTempDataStore)
        {
            _routineTempDataStore = routineTempDataStore;
            foreach (var item in Enum.GetValues(typeof(RoutineLevel)))
            {
                RoutineLevels.Add((RoutineLevel)item);
            }
            SubmitCommand = new CreateRoutineModelCommand(_routineTempDataStore, this);
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
        private RoutineLevel _level;
        public RoutineLevel Level
        {
            get { return _level; }
            set
            {
                _level = value; OnPropertyChanged(nameof(Level));
            }
        }
        public ICommand? SubmitCommand { get; }
        public ICommand? CancelCommand { get; }
        public Action? RoutineCreated;
        internal void OnRoutineCreated()
        {
            RoutineCreated?.Invoke();
        }
        #endregion

    }
}
