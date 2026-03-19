using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.LicenseManager.Models
{
    public class LicenseStatus
    {
        public string PlanName { get; set; } = "Trial";
        public bool IsFullVersion { get; set; } = false;
        public DateTime? ExpiryDate { get; set; }
        public string MachineId { get; set; } = string.Empty;

        public static LicenseStatus DefaultTrial() => new LicenseStatus
        {
            PlanName = "Trial Version",
            IsFullVersion = false,
        };
    }
}
