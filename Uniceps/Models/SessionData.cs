using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Models
{
    public class SessionData
    {
        public string Token { get; set; } = "";
        public DateTime Expiration { get; set; }
        public int UserType { get; set; }
        public string? BusinessId { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.Now;
    }
}
