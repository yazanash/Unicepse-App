using PlatinumGym.Core.Models.Employee;
//using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.SportsViewModels
{
    public class TrainersListItemViewModel : ViewModelBase
    {
        public Employee trainer;

        public TrainersListItemViewModel(Employee trainer)
        {
            this.trainer = trainer;
        }

        public string? TrainerName => trainer.FullName;
        public int TrainerAge => trainer.BirthDate;
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }

    }
}
