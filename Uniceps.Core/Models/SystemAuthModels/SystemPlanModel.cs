using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models.SystemAuthModels
{
    public class SystemPlanModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<SystemPlanItem> PlanItems { get; set; } = new();
    }
}
