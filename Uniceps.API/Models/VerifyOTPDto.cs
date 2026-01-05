using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.API.Models
{
    public class VerifyOTPDto
    {
        public string? Email { get; set; }
        public string? OTP { get; set; }
        public string DeviceToken { get; set; } = "";
        public string NotifyToken { get; set; } = "";
        public string DeviceId { get; set; } = "";
        public string Platform { get; set; } = "";
        public string AppVersion { get; set; } = "";
        public string DeviceModel { get; set; } = "";
        public string OsVersion { get; set; } = "";
    }
    public class RequestOTPDto
    {
        public string? Email { get; set; }
    }
}
