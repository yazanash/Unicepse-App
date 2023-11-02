using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Models
{
    public class User : DomainObject
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
