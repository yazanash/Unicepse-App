using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Models
{
    public class PlayerProgram : DomainObject
    {
        public virtual Sport? Sport { get; set; }
        public string? Name { get; set; }
    }
}
