using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class TrainerDausesListItemViewModel : ViewModelBase
    {
        public Subscription _subscription;
        public TrainerDausesListItemViewModel(Subscription subscription)
        {
            _subscription = subscription;
        }
        public string? PlayerName => _subscription.PlayerName;
        public string? SportName => _subscription.SportName;
        public double Price => _subscription.PriceAfterOffer;
        public string rollAndEndDate => _subscription.RollDate.ToShortDateString() + "-" + _subscription.EndDate.ToShortDateString();
        public double OfferVal => _subscription.OfferValue;
        //public string FromTo => trainerDueses.From.ToString() + "-" + trainerDueses.To.ToString();

        //public double Duses => trainerDueses.Value;
        public double dayPrice => _subscription.PriceAfterOffer / _subscription.DaysCount;

    }
}
