using Unicepse.Core.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emp = Unicepse.Core.Models.Employee;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels.SubscriptionViewModel
{
    public class SubscriptionTrainerListItem : ViewModelBase
    {
        public emp.Employee trainer;

        public SubscriptionTrainerListItem(emp.Employee trainer)
        {
            this.trainer = trainer;
        }
        public int Id => trainer.Id;
        public string? TrainerName => trainer.FullName;

    }
}
