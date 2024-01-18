using PlatinumGym.Core.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.SubscriptionViewModel
{
    public class SubscriptionTrainerListItem : ViewModelBase
    {
        public Employee trainer;

        public SubscriptionTrainerListItem(Employee trainer)
        {
            this.trainer = trainer;
        }

        public string? TrainerName => trainer.FullName;

    }
}
