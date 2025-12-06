using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.Commands.RoutineSystemCommands.DayGroupCommands;

namespace Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels
{
    public class DayGroupViewModel : ViewModelBase
    {
        private readonly DayGroupDataStore _dayGroupDataStore;
        public DayGroup DayGroup { get; set; }
        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set { _isEditing = value; OnPropertyChanged(nameof(IsEditing)); }
        }

        public ICommand? SubmitCommand { get; }
        public ICommand? DeleteCommand { get; }
        public DayGroupViewModel(DayGroup dayGroup, DayGroupDataStore dayGroupDataStore)
        {
            _dayGroupDataStore = dayGroupDataStore;

            DayGroup = dayGroup;
            Name = dayGroup.Name!;
            SubmitCommand = new UpdateDayGroupCommand(_dayGroupDataStore, this);
            DeleteCommand = new DeleteDayGroupCommand(_dayGroupDataStore,this);
        }
        public string? DayGroupName => DayGroup!.Name;
        private string? _name;
        public string? Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private int _order;
        public int Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged(nameof(Order)); }
        }

        internal void Update(DayGroup obj)
        {
            DayGroup = obj;
            Name = obj.Name!;
            IsEditing = false;
            OnPropertyChanged(nameof(DayGroupName));
        }
    }
}
