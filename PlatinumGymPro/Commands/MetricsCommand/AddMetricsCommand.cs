using PlatinumGym.Core.Models.Metric;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands.MetricsCommand
{
    public class AddMetricsCommand : AsyncCommandBase
    {
        private readonly MetricDataStore _metricDataStore;
        private readonly AddMetricsViewModel _addMetricsViewModel;
        private readonly PlayersDataStore _playerDataStore;
        private readonly NavigationService<MetricReportViewModel> _navigationService;

        public AddMetricsCommand(MetricDataStore metricDataStore, AddMetricsViewModel addMetricsViewModel, PlayersDataStore playerDataStore, NavigationService<MetricReportViewModel> navigationService)
        {
            _metricDataStore = metricDataStore;
            _addMetricsViewModel = addMetricsViewModel;
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
                CheckDate = _addMetricsViewModel.CheckDate,
                Wieght = _addMetricsViewModel.Wieght,
                Hieght = _addMetricsViewModel.Hieght,
                //Middle 
                Chest = _addMetricsViewModel.Chest,
                Hips = _addMetricsViewModel.Hips,
                Nick = _addMetricsViewModel.Nick,
                Shoulders = _addMetricsViewModel.Shoulders,
                Waist = _addMetricsViewModel.Waist,
                //ARM
                L_Arm = _addMetricsViewModel.L_Arm,
                R_Arm = _addMetricsViewModel.R_Arm,
                //Humerus
                L_Humerus = _addMetricsViewModel.L_Humerus,
                R_Humerus = _addMetricsViewModel.R_Humerus,
                //Leg
                L_Leg = _addMetricsViewModel.L_Leg,
                R_Leg = _addMetricsViewModel.R_Leg,
                //Thigh
                L_Thigh = _addMetricsViewModel.L_Thigh,
                R_Thigh = _addMetricsViewModel.R_Thigh,
                //Player
                Player = _playerDataStore.SelectedPlayer!.Player

            };
            await _metricDataStore.Add(metric);
            MessageBox.Show("Metric added successfully");
            _navigationService.ReNavigate();
        }
    }
}
