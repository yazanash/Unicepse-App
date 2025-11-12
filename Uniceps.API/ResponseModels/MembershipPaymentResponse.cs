using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.API.ResponseModels
{
    public class MembershipPaymentResponse
    {
        public bool RequirePayment { get; set; }
        public string? PaymentUrl { get;set; }
        public string? Message { get; set; }
    }
}
