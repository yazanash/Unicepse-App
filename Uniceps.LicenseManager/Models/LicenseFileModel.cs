using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.LicenseManager.Models
{
    public class LicenseFileModel
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = "";
        public int MaxDevices { get; set; }
        public string ServerSignature { get; set; } = "";
    }
}
