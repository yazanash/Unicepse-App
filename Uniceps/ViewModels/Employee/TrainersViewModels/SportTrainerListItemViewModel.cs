using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.utlis.common;
using Uniceps.Core.Models.Sport;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class SportTrainerListItemViewModel : ViewModelBase
    {
        public Sport sport;

        public SportTrainerListItemViewModel(Sport sport)
        {
            this.sport = sport;
        }
        public string? SportName => sport.Name;
        private bool _isSelected = false;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }

    }
}
