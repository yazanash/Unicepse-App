using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.API.ResponseModels
{
    public class SystemPlanResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<PlanItemResponse> PlanItems = new ();
    }
}
