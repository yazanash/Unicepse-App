using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.ViewModels.SubscriptionViewModel
{
    public class SubscriptionTrainerListItem : ViewModelBase
    {
        public Core.Models.Employee.Employee trainer;

        public SubscriptionTrainerListItem(Core.Models.Employee.Employee trainer)
        {
            this.trainer = trainer;
        }
        public int Id => trainer.Id;
        public string? TrainerName => trainer.FullName;

    }
}
