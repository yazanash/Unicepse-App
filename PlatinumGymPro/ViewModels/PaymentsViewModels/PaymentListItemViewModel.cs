using PlatinumGym.Core.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.PaymentsViewModels
{
    public class PaymentListItemViewModel : ViewModelBase
    {
        public PlayerPayment payment;
        public int Id => payment.Id;
        public int SubscriptionId => payment.Subscription!.Id;
        public string? Description => payment.Des;
        public double Value => payment.PaymentValue;
        public DateTime Date => payment.PayDate;
        public PaymentListItemViewModel(PlayerPayment payment)
        {
            this.payment = payment;
        }

        internal void Update(PlayerPayment payment)
        {
            throw new NotImplementedException();
        }
    }
}
