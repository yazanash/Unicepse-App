using Unicepse.Core.Models.Metric;
using Unicepse.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.Metrics
{
    public class MetricListItemViewModel : ViewModelBase
    {

        private readonly MetricDataStore _metricDataStore;
        private readonly NavigationStore _navigationStore;
        private readonly MetricReportViewModel _metricReportViewModel;
        private readonly PlayersDataStore _playerDataStore;

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
        public MetricListItemViewModel(Metric metric, MetricDataStore metricDataStore, NavigationStore navigationStore, MetricReportViewModel metricReportViewModel, PlayersDataStore playerDataStore)
        {
            _metricDataStore = metricDataStore;
            _navigationStore = navigationStore;
            _metricReportViewModel = metricReportViewModel;
            _playerDataStore = playerDataStore;
            Metric = metric;
            _metricDataStore.SelectedMetric = Metric;
            EditCommand = new NavaigateCommand<EditMetricsViewModel>(new NavigationService<EditMetricsViewModel>(_navigationStore, () => new EditMetricsViewModel(_metricDataStore, _navigationStore, _metricReportViewModel, _playerDataStore)));

        }
        public ICommand EditCommand { get; }
        public void Update(Metric metric)
        {
            Metric = metric;

            //OnPropertyChanged(nameof(SportName));
        }
    }
}
