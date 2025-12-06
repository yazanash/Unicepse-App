using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.API.ResponseModels
{
    public class VerifyOtpResponse
    {
        public string? Token { get; set; }
        public int UserType { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string? Id { get; set; }
    }
}
