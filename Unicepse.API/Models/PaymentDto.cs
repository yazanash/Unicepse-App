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
        public int id { get; set; }
        public int pid { get; set; }
        public int sid { get; set; }
        public int gym_id { get; set; }
        public double value { get; set; }
        public string? description { get; set; }
        public string? date { get; set; }

        internal void FromPayment(PlayerPayment entity)
        {
            id = entity.Id;
            pid = entity.Player!.Id;
            sid = entity.Subscription!.Id;
            value = entity.PaymentValue;
            description = entity.Des;
            date = entity.PayDate.ToString("dd/MM/yyyy");

        }

        internal PlayerPayment ToPayment()
        {
            PlayerPayment playerPayment = new PlayerPayment()
            {
                Id = id,
                Player = new Player() { Id = pid },
                Subscription = new Subscription() { Id = sid },
                PaymentValue = value,
                Des = description,


            };
            if (!string.IsNullOrEmpty(date))
                playerPayment.PayDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            
            return playerPayment;
        }
    }
}
