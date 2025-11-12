using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;

namespace Uniceps.API.Models
{
    public class SystemProfileDto
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderType Gender { get; set; }
        public string? PictureUrl { get; set; }
    }
    
}
