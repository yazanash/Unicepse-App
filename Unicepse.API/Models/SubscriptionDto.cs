using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Sport;
using Unicepse.Core.Models.Subscription;

namespace Unicepse.API.Models
{
    public class SubscriptionDto
    {
        public int id { get; set; }
        public int pid { get; set; }
        public int gym_id { get; set; }
        public string? sport_name { get; set; }
        public string? trainer_name { get; set; }
        public string? start_date { get; set; }
        public string? end_date { get; set; }
        public double price { get; set; }
        public double discount_value { get; set; }
        public string? discount_des { get; set; }
        public string? is_paid { get; set; }
        public double paid_value { get; set; }
        public Dictionary<string, string> payments = new Dictionary<string,string>();

        internal void FromSubscription(Subscription entity)
        {
            id = entity.Id;
            pid = entity.Player!.Id;
            sport_name = entity.Sport!.Name;
            trainer_name = entity.Trainer != null ? entity.Trainer.FullName : "";
            start_date = entity.RollDate.ToString("dd/MM/yyyy");
            end_date = entity.EndDate.ToString("dd/MM/yyyy");
            price = entity.Price;
            discount_value = entity.OfferValue;
            discount_des = entity.OfferDes;
            is_paid = entity.IsPaid.ToString();
            paid_value = entity.PaidValue;
            payments.Add("test", "test");
        }

        internal Subscription ToSubscription()
        {
            Subscription subscription = new Subscription()
            {

                Id = id,
                Player = new Player { Id = pid },
                Sport = new Sport { Name = sport_name } ,
                Price = price,
                OfferValue = discount_value,
                OfferDes = discount_des,
                IsPaid = Convert.ToBoolean( is_paid),
                PaidValue = paid_value,

            };
            //if (!string.IsNullOrEmpty(trainer_name))
                subscription.Trainer = new Employee { FullName = trainer_name };
            if (!string.IsNullOrEmpty(start_date) && !string.IsNullOrEmpty(end_date))
            {
                subscription.RollDate = DateTime.ParseExact(start_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                subscription.EndDate = DateTime.ParseExact(end_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
               
            return subscription;
        }
    }
}
