using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels
{
    public class EquipmentListItemViewModel : ViewModelBase
    {
        public Equipment Equipment;

        public EquipmentListItemViewModel(Equipment equipment)
        {
            Equipment = equipment;
        }
        public string Code => Equipment.Code;
        public string Name => Equipment.Name;
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }
    }
}
