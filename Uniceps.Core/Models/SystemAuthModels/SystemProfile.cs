using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;

namespace Uniceps.Core.Models.SystemAuthModels
{
    public class SystemProfile
    {
        public Guid Id { get; set; }
        public string? BusinessId { get; set; }
        public string DisplayName { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public string OwnerName { get; set; } = "";
        public string? ProfileImagePath { get; set; }
        public string? LocalProfileImagePath { get; set; }
        public GenderType Gender { get; set; }
    }

}
