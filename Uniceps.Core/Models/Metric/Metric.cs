using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Payment;

namespace Uniceps.Core.Models.Metric
{
    public class Metric : DomainObject
    {
        public Player.Player? Player { get; set; }
        public int PlayerId { get; set; }
        public Guid PlayerSyncId { get; set; }
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

        public void MergeWith(Metric metric)
        {
            PlayerId = metric.PlayerId;
            PlayerSyncId = metric.PlayerSyncId;
            Hieght = metric.Hieght;
            Wieght = metric.Wieght;
            L_Arm = metric.L_Arm;
            R_Arm = metric.R_Arm;
            L_Humerus = metric.L_Humerus;
            R_Humerus = metric.R_Humerus;
            L_Thigh = metric.L_Thigh;
            R_Thigh = metric.R_Thigh;
            L_Leg = metric.L_Leg;
            R_Leg = metric.R_Leg;
            Nick = metric.Nick;
            Shoulders = metric.Shoulders;
            Waist = metric.Waist;
            Chest = metric.Chest;
            Hips = metric.Hips;
            CheckDate = metric.CheckDate;
            SyncId = metric.SyncId;
        }
    }
}
