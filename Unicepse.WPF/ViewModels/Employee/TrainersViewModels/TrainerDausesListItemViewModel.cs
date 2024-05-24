using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.WPF.utlis.common;

namespace Unicepse.WPF.ViewModels.Employee.TrainersViewModels
{
    public class TrainerDausesListItemViewModel : ViewModelBase
    {
        public Subscription _subscription;
        public TrainerDausesListItemViewModel(Subscription subscription)
        {
            _subscription = subscription;
        }
        public string? PlayerName => _subscription.Player!.FullName;
        public string? SportName => _subscription.Sport!.Name;
        public double Price => _subscription.PriceAfterOffer;
        public string rollAndEndDate => _subscription.RollDate.ToShortDateString() + "-" + _subscription.EndDate.ToShortDateString();
        public double Paid => _subscription.PaidValue;
        public double OfferVal => _subscription.OfferValue;
        //public string FromTo => trainerDueses.From.ToString() + "-" + trainerDueses.To.ToString();
        
        //public double Duses => trainerDueses.Value;
        public double dayPrice => _subscription.PriceAfterOffer / _subscription.DaysCount;

    }
}
