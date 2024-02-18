using PlatinumGym.Core.Models.Metric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.Metrics
{
    public class MetricListItemViewModel : ViewModelBase
    {
        public Metric Metric;

        public double? Hieght => ((int?)Metric.Hieght);
        public double? Wieght => ((int?)Metric.Wieght);
        public double? L_Arm => ((int?)Metric.L_Arm);
        public double? R_Arm => ((int?)Metric.R_Arm);
        public double? L_Humerus => ((int?)Metric.L_Humerus);
        public double? R_Humerus => ((int?)Metric.R_Humerus);
        public double? L_Thigh => ((int?)Metric.L_Thigh);
        public double? R_Thigh => ((int?)Metric.R_Thigh);
        public double? L_Leg => ((int?)Metric.L_Leg);
        public double? R_Leg => ((int?)Metric.R_Leg);
        public double? Nick => ((int?)Metric.Nick);
        public double? Shoulders => ((int?)Metric.Shoulders);
        public double? Waist => ((int?)Metric.Waist);
        public double? Chest => ((int?)Metric.Chest);
        public double? Hips => ((int?)Metric.Hips);
        public string? CheckDate => Metric.CheckDate.ToString("ddd,MMM dd,yyy");
        public MetricListItemViewModel(Metric metric)
        {
            Metric = metric;
        }

        public void Update(Metric metric)
        {
            this.Metric = metric;

            //OnPropertyChanged(nameof(SportName));
        }
    }
}
