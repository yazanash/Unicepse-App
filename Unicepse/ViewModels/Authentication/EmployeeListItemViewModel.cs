using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;
using emp = Unicepse.Core.Models.Employee;
namespace Unicepse.ViewModels.Authentication
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
