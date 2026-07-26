using Uniceps.Commands;
using Uniceps.Commands.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.navigation.Stores;
using Uniceps.Core.Models.Payment;

namespace Uniceps.ViewModels.PaymentsViewModels
{
    public class PaymentListItemViewModel : ViewModelBase
    {

        public PlayerPayment payment;

        private int _order;
        public int Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged(nameof(Order)); }
        }
        public int Id => payment.Id;
        public string? Description => payment.Des;
        public double Value => payment.PaymentValue;
        public string? Date => payment.PayDate.ToShortDateString();
        public bool IsEdited => payment.CreatedAt != payment.UpdatedAt;
        public string? CreatedDate => payment.CreatedAt.ToString("yyyy-M-dd hh:mm:ss");
        public string? EditedDate => payment.UpdatedAt.ToString("yyyy-M-dd hh:mm:ss");
        public PaymentListItemViewModel(PlayerPayment payment)
        {
            this.payment = payment;
        }
        public void Update(PlayerPayment payment)
        {
            this.payment = payment;
        }
    }
}
