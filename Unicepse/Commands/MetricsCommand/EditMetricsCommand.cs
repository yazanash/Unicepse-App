using Unicepse.Core.Models.Metric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.ViewModels.Metrics;
using Unicepse.navigation;

namespace Unicepse.Commands.MetricsCommand
{
    public class EditMetricsCommand : AsyncCommandBase
    {
        private readonly MetricDataStore _metricDataStore;
        private readonly EditMetricsViewModel _editMetricsViewModel;
        private readonly PlayersDataStore _playerDataStore;
        private readonly NavigationService<MetricReportViewModel> _navigationService;

        public EditMetricsCommand(MetricDataStore metricDataStore, EditMetricsViewModel editMetricsViewModel, PlayersDataStore playerDataStore, NavigationService<MetricReportViewModel> navigationService)
        {
            _metricDataStore = metricDataStore;
            _editMetricsViewModel = editMetricsViewModel;
            _playerDataStore = playerDataStore;
            _navigationService = navigationService;
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {

            Metric metric = new Metric()
            {
                Id = _metricDataStore.SelectedMetric!.Id,
                CheckDate = _editMetricsViewModel.CheckDate,
                Wieght = _editMetricsViewModel.Wieght,
                Hieght = _editMetricsViewModel.Hieght,
                //Middle 
                Chest = _editMetricsViewModel.Chest,
                Hips = _editMetricsViewModel.Hips,
                Nick = _editMetricsViewModel.Nick,
                Shoulders = _editMetricsViewModel.Shoulders,
                Waist = _editMetricsViewModel.Waist,
                //ARM
                L_Arm = _editMetricsViewModel.L_Arm,
                R_Arm = _editMetricsViewModel.R_Arm,
                //Humerus
                L_Humerus = _editMetricsViewModel.L_Humerus,
                R_Humerus = _editMetricsViewModel.R_Humerus,
                //Leg
                L_Leg = _editMetricsViewModel.L_Leg,
                R_Leg = _editMetricsViewModel.R_Leg,
                //Thigh
                L_Thigh = _editMetricsViewModel.L_Thigh,
                R_Thigh = _editMetricsViewModel.R_Thigh,
                //Player
                Player = _playerDataStore.SelectedPlayer!.Player

            };
            await _metricDataStore.Update(metric);
            _navigationService.ReNavigate();
        }
    }
}
