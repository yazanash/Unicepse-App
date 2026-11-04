using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Uniceps.Commands;
using Uniceps.Core.Models.Metric;
using Uniceps.navigation;
using Uniceps.navigation.NavigationSystem;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.TrainersViewModels;
using Uniceps.ViewModels.Metrics;

namespace Uniceps.Commands.MetricsCommand
{
    public class AddMetricsCommand : AsyncCommandBase
    {
        private readonly MetricDataStore _metricDataStore;
        private readonly AddMetricsViewModel _addMetricsViewModel;

        public AddMetricsCommand(MetricDataStore metricDataStore, AddMetricsViewModel addMetricsViewModel)
        {
            _metricDataStore = metricDataStore;
            _addMetricsViewModel = addMetricsViewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            try
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
                    PlayerId = _addMetricsViewModel.PlayerId,

                };
                if (_addMetricsViewModel.IsEditMode && _addMetricsViewModel._metricId != -1)
                {
                    metric.Id = _addMetricsViewModel._metricId;
                    await _metricDataStore.Update(metric);
                }
                else
                    await _metricDataStore.Add(metric);
                _addMetricsViewModel.OnSave();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
