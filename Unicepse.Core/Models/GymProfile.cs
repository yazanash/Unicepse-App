using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Core.Models
{
    public class GymProfile : DomainObject
    {
        public string? GymId { get; set; }
        public string? GymName { get; set; }
        public string? OwnerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Telephone { get; set; }
        public string? Logo { get; set; }
        public string? Address { get; set; }
    }
}
