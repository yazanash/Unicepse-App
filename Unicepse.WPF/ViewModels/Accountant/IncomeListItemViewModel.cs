using Unicepse.Core.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.WPF.utlis.common;

namespace Unicepse.WPF.ViewModels.Accountant
{
    public class IncomeListItemViewModel : ViewModelBase
    {
        public PlayerPayment playerPayment;
        public int Id => playerPayment.Id;
        public string? PlayerName => playerPayment.Player!.FullName;
        public string? SportName => playerPayment.Subscription!.Sport!.Name;
        public string? TrainerName => playerPayment.Subscription!.Trainer!=null? playerPayment.Subscription!.Trainer!.FullName:"بدون مدرب";
        public double SubscriptionPrice => playerPayment.Subscription!.PriceAfterOffer;
        public string? IssueDate => playerPayment.PayDate.ToShortDateString();
        public double PayVal => playerPayment.PaymentValue;
        public IncomeListItemViewModel(PlayerPayment playerPayment)
        {
            this.playerPayment = playerPayment;
        }
    }
}
