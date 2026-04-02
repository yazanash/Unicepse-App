using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.LicenseManager.Models
{
    public class ActivationResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; } = "";
        public DateTime ActivatedAt { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public string Message { get; set; } = "";
    }
}
