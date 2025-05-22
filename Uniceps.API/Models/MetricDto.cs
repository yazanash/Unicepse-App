using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Metric;

namespace Uniceps.API.Models
{
    public class MetricDto
    {
        public string? id { get; set; }
        public string? pid { get; set; }
        public string? gym_id { get; set; }
        public double height { get; set; }
        public double weight { get; set; }
        public double l_arm { get; set; }
        public double r_arm { get; set; }
        public double l_humerus { get; set; }
        public double r_humerus { get; set; }
        public double l_thigh { get; set; }
        public double r_thigh { get; set; }
        public double l_leg { get; set; }
        public double r_leg { get; set; }
        public double neck { get; set; }
        public double shoulders { get; set; }
        public double waist { get; set; }
        public double chest { get; set; }
        public double hips { get; set; }
        public DateTime check_date { get; set; }

        public Metric ToMetric()
        {
            Metric metric = new Metric()
            {
                Id = Convert.ToInt32(id),
                Player = new Core.Models.Player.Player() { Id = Convert.ToInt32(pid) },
                Hieght = height,
                Wieght = weight,
                L_Arm = l_arm,
                R_Arm = r_arm,
                L_Humerus = l_humerus,
                R_Humerus = r_humerus,
                L_Thigh = l_thigh,
                R_Thigh = r_thigh,
                L_Leg = l_leg,
                R_Leg = r_leg,
                Nick = neck,
                Shoulders = shoulders,
                Waist = waist,
                Chest = chest,
                Hips = hips,
                CheckDate = check_date.ToLocalTime()
            };
            return metric;
        }
        public void FromMetric(Metric entity)
        {
            id = entity.Id.ToString();
            pid = entity.Player!.Id.ToString();
            height = entity.Hieght;
            weight = entity.Wieght;
            l_arm = entity.L_Arm;
            r_arm = entity.R_Arm;
            l_humerus = entity.L_Humerus;
            r_humerus = entity.R_Humerus;
            l_thigh = entity.L_Thigh;
            r_thigh = entity.R_Thigh;
            l_leg = entity.L_Leg;
            r_leg = entity.R_Leg;
            neck = entity.Nick;
            shoulders = entity.Shoulders;
            waist = entity.Waist;
            chest = entity.Chest;
            hips = entity.Hips;
            check_date = entity.CheckDate.ToUniversalTime();
        }
    }
}
