using PlatinumGym.Core.Models.Employee;
//using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emp = PlatinumGym.Core.Models.Employee;
namespace PlatinumGymPro.ViewModels.SportsViewModels
{
    public class TrainersListItemViewModel : ViewModelBase
    {
        public emp.Employee trainer;

        public TrainersListItemViewModel(emp.Employee trainer)
        {
            this.trainer = trainer;
        }
        public string? TrainerName => trainer.FullName;
        public int TrainerAge => trainer.BirthDate;
        private bool _isSelected = false;

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }

    }
}
