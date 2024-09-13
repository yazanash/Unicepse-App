using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Subscription;

namespace Unicepse.API.Models
{
    public class PaymentDto
    {
        public string? id { get; set; }
        public string? pid { get; set; }
        public string?  sid { get; set; }
        public string? gym_id { get; set; }
        public double value { get; set; }
        public string? description { get; set; }
        public string? date { get; set; }

        internal void FromPayment(PlayerPayment entity)
        {
            id = entity.Id.ToString();
            pid = entity.Player!.Id.ToString();
            sid = entity.Subscription!.Id.ToString();
            value = entity.PaymentValue;
            description = entity.Des;
            date = entity.PayDate.ToString("dd/MM/yyyy");

        }

        internal PlayerPayment ToPayment()
        {
            PlayerPayment playerPayment = new PlayerPayment()
            {
                Id =Convert.ToInt32( id),
                Player = new Player() { Id = Convert.ToInt32( pid) },
                Subscription = new Subscription() { Id = Convert.ToInt32( sid) },
                PaymentValue = value,
                Des = description,


            };
            if (!string.IsNullOrEmpty(date))
                playerPayment.PayDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            
            return playerPayment;
        }
    }
}
