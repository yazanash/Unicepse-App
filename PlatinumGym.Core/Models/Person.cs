using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Core.Models
{
    public abstract class Person : DomainObject
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public int BirthDate { get; set; }
        public bool GenderMale { get; set; }
    }
}
