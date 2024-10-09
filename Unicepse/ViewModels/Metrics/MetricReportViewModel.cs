using Unicepse.Core.Models.Metric;
using Unicepse.Commands;
using Unicepse.Commands.MetricsCommand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.Stores;
using Unicepse.ViewModels;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.Metrics
{
    public class MetricReportViewModel : ListingViewModelBase
    {
        private readonly MetricDataStore _metricDataStore;
        private readonly PlayersDataStore _playerDataStore;
        private readonly NavigationStore _navigationStore;
        private ObservableCollection<MetricListItemViewModel> _metricListItemViewModels;

        public IEnumerable<MetricListItemViewModel> Metrics => _metricListItemViewModels;
        public MetricListItemViewModel? SelectedMetric
        {
            get
            {
                return Metrics
                    .FirstOrDefault(y => y?.Metric == _metricDataStore.SelectedMetric);
            }
            set
            {
                _metricDataStore.SelectedMetric = value?.Metric;
                OnPropertyChanged(nameof(SelectedMetric));

            }
        }
        public ICommand LoadMetricCommand { get; }
        public ICommand AddMetricsCommand { get; }
        public MetricReportViewModel(MetricDataStore metricDataStore, PlayersDataStore playerDataStore, NavigationStore navigationStore)
        {
            _metricDataStore = metricDataStore;
            _playerDataStore = playerDataStore;
            _navigationStore = navigationStore;
            _metricListItemViewModels = new ObservableCollection<MetricListItemViewModel>();
            _metricDataStore.Loaded += _metricDataStore_Loaded;
            _metricDataStore.Created += _metricDataStore_Created;
            _metricDataStore.Updated += _metricDataStore_Updated;
            _metricDataStore.Deleted += _metricDataStore_Deleted;
            LoadMetricCommand = new LoadMetricsCommand(this, _metricDataStore, _playerDataStore!.SelectedPlayer!);
            AddMetricsCommand = new NavaigateCommand<AddMetricsViewModel>(new NavigationService<AddMetricsViewModel>(_navigationStore, () => new AddMetricsViewModel(_metricDataStore, _navigationStore, this, _playerDataStore)));
            SelectedMetric = _metricListItemViewModels.FirstOrDefault();
        }

        private void _metricDataStore_Deleted(int id)
        {
            MetricListItemViewModel? itemViewModel = _metricListItemViewModels.FirstOrDefault(y => y.Metric?.Id == id);

            if (itemViewModel != null)
            {
                _metricListItemViewModels.Remove(itemViewModel);
            }
        }

        private void _metricDataStore_Updated(Metric obj)
        {
            MetricListItemViewModel? metricViewModel =
                    _metricListItemViewModels.FirstOrDefault(y => y.Metric.Id == obj.Id);

            if (metricViewModel != null)
            {
                metricViewModel.Update(obj);
            }
        }

        private void _metricDataStore_Created(Metric obj)
        {
            AddMetric(obj);
        }

        private void _metricDataStore_Loaded()
        {
            _metricListItemViewModels.Clear();
            foreach (var metric in _metricDataStore.Metrics.OrderByDescending(x => x.CheckDate))
            {
                AddMetric(metric);

            }
            SelectedMetric = Metrics.FirstOrDefault();
        }
        public bool IsSelected = true;
        private void AddMetric(Metric metric)
        {
            MetricListItemViewModel viewmodel = new(metric, _metricDataStore, _navigationStore, this, _playerDataStore);
            _metricListItemViewModels.Add(viewmodel);
        }

        public static MetricReportViewModel LoadViewModel(MetricDataStore metricDataStore, PlayersDataStore playerDataStore, NavigationStore navigationStore)
        {
            MetricReportViewModel viewModel = new(metricDataStore, playerDataStore, navigationStore);

            viewModel.LoadMetricCommand.Execute(null);

            return viewModel;
        }
    }
}
