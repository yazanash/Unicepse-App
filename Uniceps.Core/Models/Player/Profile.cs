using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models.Player
{
    public class Profile
    {
        public string? UID { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public int BirthDate { get; set; }
        public bool GenderMale { get; set; }
    }
}
