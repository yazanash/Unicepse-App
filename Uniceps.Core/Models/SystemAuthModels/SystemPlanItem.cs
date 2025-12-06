using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models.SystemAuthModels
{
    public class SystemPlanItem
    {
        public int Id { get; set; }
        public string? DurationString { get; set; }
        public decimal Price { get; set; }
        public int DaysCount { get; set; }
        public bool IsFree { get; set; }
    }
}
