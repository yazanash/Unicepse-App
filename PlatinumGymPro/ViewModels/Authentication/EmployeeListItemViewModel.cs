using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emp = PlatinumGym.Core.Models.Employee;
namespace PlatinumGymPro.ViewModels.Authentication
{
    public class EmployeeListItemViewModel : ViewModelBase
    {
        public emp.Employee trainer;

        public EmployeeListItemViewModel(emp.Employee trainer)
        {
            this.trainer = trainer;
        }
        public int Id => trainer.Id;
        public string? TrainerName => trainer.FullName;

    }
}
