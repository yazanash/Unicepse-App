using PlatinumGym.Core.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emp = PlatinumGym.Core.Models.Employee;
namespace PlatinumGymPro.ViewModels.SubscriptionViewModel
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
