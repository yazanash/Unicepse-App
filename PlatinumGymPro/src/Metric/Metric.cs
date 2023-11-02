using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Models
{
    public class Metric : DomainObject
    {
        public Player? Player { get; set; }
        
        public string? Hieght { get; set; }
        public string? Wieght { get; set; }
        public string? L_Arm { get; set; }
        public string? R_Arm { get; set; }
        public string? L_Humerus { get; set; }
        public string? R_Humerus { get; set; }
        public string? L_Thigh { get; set; }
        public string? R_Thigh { get; set; }
        public string? L_Leg { get; set; }
        public string? R_Leg { get; set; }
        public string? Nick { get; set; }
        public string? Shoulders { get; set; }
        public string? Waist { get; set; }
        public string? Chest { get; set; }
        public string? Hips { get; set; }
        public DateTime CheckDate { get; set; }
    }
}
