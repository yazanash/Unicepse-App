using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;

namespace Uniceps.API.Models
{
    public class SubscriptionDto
    {
        public string? id { get; set; }
        public string? pid { get; set; }
        public string? gym_id { get; set; }
        public string? sport_name { get; set; }
        public string? trainer_name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public double price { get; set; }
        public double discount_value { get; set; }
        public string? discount_des { get; set; }
        public string? is_paid { get; set; }
        public double paid_value { get; set; }

        public void FromSubscription(Subscription entity)
        {
            id = entity.Id.ToString();
            pid = entity.Player!.Id.ToString();
            sport_name = entity.Sport!.Name;
            trainer_name = entity.Trainer != null ? entity.Trainer.FullName : "";
            start_date = entity.RollDate.ToUniversalTime();
            end_date = entity.EndDate!.ToUniversalTime();
            price = entity.Price;
            discount_value = entity.OfferValue;
            discount_des = entity.OfferDes;
            is_paid = entity.IsPaid.ToString();
            paid_value = entity.PaidValue;
        }

        public Subscription ToSubscription()
        {
            Subscription subscription = new Subscription()
            {

                Id = Convert.ToInt32(id),
                Player = new Player { Id = Convert.ToInt32(pid) },
                Sport = new Sport { Name = sport_name },
                Price = price,
                OfferValue = discount_value,
                OfferDes = discount_des,
                IsPaid = Convert.ToBoolean(is_paid),
                PaidValue = paid_value,
                RollDate = start_date.ToLocalTime(),
                EndDate = end_date.ToLocalTime()
            };
            //if (!string.IsNullOrEmpty(trainer_name))
            subscription.Trainer = new Employee { FullName = trainer_name };
            return subscription;
        }
    }
}
