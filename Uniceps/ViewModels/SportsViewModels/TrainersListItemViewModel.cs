using Uniceps.Core.Models.Employee;
//using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emp = Uniceps.Core.Models.Employee;

namespace Uniceps.ViewModels.SportsViewModels
{
    public class TrainersListItemViewModel : ViewModelBase
    {
        public Emp.Employee trainer;

        public TrainersListItemViewModel(Emp.Employee trainer)
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
