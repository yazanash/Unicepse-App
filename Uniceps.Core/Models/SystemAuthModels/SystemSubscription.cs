using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models.SystemAuthModels
{
    public class SystemSubscription
    {
        public int Id { get; set; }
        public Guid PublicId { get; set; }
        public string? PlanName { get; set; }
        public decimal Price {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsGift { get; set; }
    }
}
