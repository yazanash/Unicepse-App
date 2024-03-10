using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlatinumGym.Core.Models.Sport;

namespace PlatinumGym.Core.Models.Subscription
{
    public class Subscription : DomainObject
    {
        public virtual Sport.Sport? Sport { get; set; }
        public DateTime LastCheck { get; set; }
        public int? TrainerId { get; set; }
        public virtual Employee.Employee? Trainer { get; set; }
        public int PrevTrainer_Id { get; set; }
        public virtual Player.Player? Player { get; set; }
        public DateTime RollDate { get; set; }
        public double Price { get; set; }
        public  double OfferValue { get; set; }
        public string? OfferDes{ get; set; }
        public double PriceAfterOffer { get; set; }
        public int MonthCount { get; set; }
        public int DaysCount { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsPlayerPay { get; set; }
        public bool IsStopped { get; set; }
        public bool IsMoved { get; set; }
        public double PrivatePrice { get; set; }
        public bool IsPaid { get; set; }
        public double PaidValue { get; set; }
        public double RestValue { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime LastPaid { get; set; }
    }
}
