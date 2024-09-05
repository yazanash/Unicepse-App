﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;
using Unicepse.Core.Models.Player;
namespace Unicepse.Core.Models.Metric
{
    public class Metric : DomainObject
    {
        public Player.Player? Player { get; set; }
        public DataStatus DataStatus { get; set; }
        public double Hieght { get; set; }
        public double Wieght { get; set; }
        public double L_Arm { get; set; }
        public double R_Arm { get; set; }
        public double L_Humerus { get; set; }
        public double R_Humerus { get; set; }
        public double L_Thigh { get; set; }
        public double R_Thigh { get; set; }
        public double L_Leg { get; set; }
        public double R_Leg { get; set; }
        public double Nick { get; set; }
        public double Shoulders { get; set; }
        public double Waist { get; set; }
        public double Chest { get; set; }
        public double Hips { get; set; }
        public DateTime CheckDate { get; set; }
    }
}
