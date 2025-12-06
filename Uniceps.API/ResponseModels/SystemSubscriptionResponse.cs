using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.API.ResponseModels
{
    public class SystemSubscriptionResponse
    {
        public Guid Id { get; set; }
        public string? Plan { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsGift { get; set; }
        public string? SessionUrl { get; set; }
    }
}
