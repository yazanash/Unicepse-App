﻿using Unicepse.Core.Models.Metric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.ViewModels.Metrics;
using Unicepse.Stores;
using Unicepse.navigation;

namespace Unicepse.Commands.MetricsCommand
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
            if(!_playerDataStore.SelectedPlayer!.Player.IsSubscribed)
            {
                _playerDataStore.SelectedPlayer!.Player.IsSubscribed = true;
                await _playerDataStore.UpdatePlayer(_playerDataStore.SelectedPlayer!.Player);
                if (_playerDataStore.SelectedPlayer.Player != null)
                {
                    _playerDataStore.SelectedPlayer.IsActive = true;
                }
            }
            await _metricDataStore.Add(metric);
            _metricDataStore.SelectedMetric = _metricDataStore.Metrics.FirstOrDefault(x=>x.Id==metric.Id);
            _navigationService.ReNavigate();
        }
    }
}
