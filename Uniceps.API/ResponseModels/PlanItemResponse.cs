using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.API.ResponseModels
{
    public class PlanItemResponse
    {
        public int Id { get; set; }
        public string? DurationString { get; set; }
        public decimal Price { get; set; }
        public int DaysCount { get; set; }
        public bool IsFree { get; set; }
    }
}
