using Uniceps.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.utlis.common;
using Uniceps.Stores;
using Uniceps.navigation.Stores;
using Uniceps.Core.Models.Metric;

namespace Uniceps.ViewModels.Metrics
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
            EditCommand = new NavaigateCommand<EditMetricsViewModel>(new NavigationService<EditMetricsViewModel>(_navigationStore, () => EditMetrics()));

        }

        private EditMetricsViewModel EditMetrics()
        {
            _metricDataStore.SelectedMetric = Metric;
            return new EditMetricsViewModel(_metricDataStore, _navigationStore, _metricReportViewModel, _playerDataStore);
        }

        public ICommand EditCommand { get; }
        public void Update(Metric metric)
        {
            Metric = metric;

            //OnPropertyChanged(nameof(SportName));
        }
    }
}
