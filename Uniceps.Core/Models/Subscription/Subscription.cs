using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Payment;

namespace Uniceps.Core.Models.Subscription
{
    public class Subscription : DomainObject
    {
        public int PlayerId { get; set; }
        public int SportId { get; set; }
        public int? TrainerId { get; set; }
        public string? PlayerName { get; set; } = string.Empty;
        public string? SportName { get; set; } = string.Empty;
        public string? TrainerName { get; set; }
        public DateTime LastCheck { get; set; }
        public DateTime RollDate { get; set; }
        public double Price { get; set; }
        public double OfferValue { get; set; }
        public string? OfferDes { get; set; }
        public double PriceAfterOffer { get; set; }
        public int MonthCount { get; set; }
        public int DaysCount { get; set; }
        public bool IsStopped { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsRenewed { get;set; }
        public List<PlayerPayment>? Payments { get; set; } = new();
        public double TotalPaid => Payments?.Sum(p => p.PaymentValue) ?? 0;
        public double Remaining => PriceAfterOffer - TotalPaid;
        public string? Code { get; set; }

        public string GenerateSubscriptionCode()
        {
            // دمج رقم اللاعب مع رقم الاشتراك
            long raw = (long)PlayerId * 1000000 + Id;

            // تحويل الناتج إلى 6 أرقام فقط باستخدام Modulo
            string code = (raw % 1000000).ToString("D6");

            return code;
        }
    }
}
