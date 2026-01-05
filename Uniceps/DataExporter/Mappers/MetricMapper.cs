using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Metric;
using Uniceps.Core.Models.Player;
using Uniceps.DataExporter.Dtos;

namespace Uniceps.DataExporter.Mappers
{
    public static class MetricMapper
    {
        public static Metric FromDto(MetricDto data)
        {
            Metric metric = new Metric()
            {
                PlayerSyncId = data.PlayerSyncId,
                Hieght = data.Hieght,
                Wieght = data.Wieght,
                L_Arm = data.L_Arm,
                R_Arm = data.R_Arm,
                L_Humerus = data.L_Humerus,
                R_Humerus = data.R_Humerus,
                L_Thigh = data.L_Thigh,
                R_Thigh = data.R_Thigh,
                L_Leg = data.L_Leg,
                R_Leg = data.R_Leg,
                Nick = data.Nick,
                Shoulders = data.Shoulders,
                Waist = data.Waist,
                Chest = data.Chest,
                Hips = data.Hips,
                CheckDate = data.CheckDate,
                CreatedAt = data.CreatedAt,
                SyncId = data.SyncId,
                UpdatedAt = data.UpdatedAt
            };
            return metric;
        }

        public static MetricDto ToDto(Metric data)
        {
            MetricDto metric = new MetricDto()
            {
                PlayerSyncId = data.PlayerSyncId,
                Hieght = data.Hieght,
                Wieght = data.Wieght,
                L_Arm = data.L_Arm,
                R_Arm = data.R_Arm,
                L_Humerus = data.L_Humerus,
                R_Humerus = data.R_Humerus,
                L_Thigh = data.L_Thigh,
                R_Thigh = data.R_Thigh,
                L_Leg = data.L_Leg,
                R_Leg = data.R_Leg,
                Nick = data.Nick,
                Shoulders = data.Shoulders,
                Waist = data.Waist,
                Chest = data.Chest,
                Hips = data.Hips,
                CheckDate = data.CheckDate,
                CreatedAt = data.CreatedAt,
                SyncId = data.SyncId,
                UpdatedAt = data.UpdatedAt,
            };
            return metric;
        }
    }
}
