using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Uniceps.ViewModels.Authentication
{
    public class EmployeeListItemViewModel : ViewModelBase
    {
        public Core.Models.Employee.Employee trainer;

        public EmployeeListItemViewModel(Core.Models.Employee.Employee trainer)
        {
            this.trainer = trainer;
        }
        public int Id => trainer.Id;
        public string? TrainerName => trainer.FullName;

    }
}
