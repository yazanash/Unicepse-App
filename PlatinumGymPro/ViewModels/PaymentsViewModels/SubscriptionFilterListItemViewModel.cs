using PlatinumGym.Core.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.PaymentsViewModels
{
    public class SubscriptionFilterListItemViewModel : ViewModelBase
    {
        public Subscription subscription;

        public string? SportName => subscription.Sport!.Name;
        public string? TrainerName => subscription.Trainer != null ? subscription.Trainer.FullName : "لايوجد مدرب";
        public string RollDate => subscription.RollDate.ToString("ddd,MMM dd,yyy");
        public SubscriptionFilterListItemViewModel(Subscription subscription)
        {
            this.subscription = subscription;
        }
    }
}
