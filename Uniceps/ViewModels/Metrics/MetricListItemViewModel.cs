using System.Collections.Generic;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Core.Models.Metric;
using Uniceps.Core.Models.Player;
using Uniceps.navigation.DialogService;
using Uniceps.navigation.NavigationSystem;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Views.Metrics;

namespace Uniceps.ViewModels.Metrics
{
    public class MetricListItemViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        public Metric Metric;

        public double Hieght => (int)Metric.Hieght;
        public double Wieght => (int)Metric.Wieght;
        public double L_Arm => (int)Metric.L_Arm;
        public double R_Arm => (int)Metric.R_Arm;
        public double L_Humerus => (int)Metric.L_Humerus;
        public double R_Humerus => (int)Metric.R_Humerus;
        public double L_Thigh => (int)Metric.L_Thigh;
        public double R_Thigh => (int)Metric.R_Thigh;
        public double L_Leg => (int)Metric.L_Leg;
        public double R_Leg => (int)Metric.R_Leg;
        public double Nick => (int)Metric.Nick;
        public double Shoulders => (int)Metric.Shoulders;
        public double Waist => (int)Metric.Waist;
        public double Chest => (int)Metric.Chest;
        public double Hips => (int)Metric.Hips;
        public string? CheckDate => Metric.CheckDate.ToShortDateString();
        public MetricListItemViewModel(Metric metric, IDialogService dialogService)
        {
            _dialogService = dialogService;
            Metric = metric;

            var navParams = new Dictionary<string, object>
            {
                {"Metric",Metric},

            };
            EditCommand = new RelayCommand(() => _dialogService.ShowDialog<AddMetricWindowView,AddMetricsViewModel>(navParams));
        }


        public ICommand EditCommand { get; }
        public void Update(Metric metric)
        {
            Metric = metric;
        }
    }
}
