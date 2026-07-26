using Uniceps.Commands;
using Uniceps.Commands.MetricsCommand;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.ViewModels;
using Uniceps.Stores;
using Uniceps.navigation.Stores;
using Uniceps.Core.Models.Metric;

namespace Uniceps.ViewModels.Metrics
{
    public class MetricReportViewModel : ListingViewModelBase
    {
        private readonly MetricDataStore _metricDataStore;
        private readonly NavigationStore _navigationStore;
        private ObservableCollection<MetricListItemViewModel> _metricListItemViewModels;
        public int PlayerId;
        public IEnumerable<MetricListItemViewModel> Metrics => _metricListItemViewModels;
        private MetricListItemViewModel? _selectedMetric;
        public MetricListItemViewModel? SelectedMetric
        {
            get
            {
                return _selectedMetric;
            }
            set
            {
                _selectedMetric = value;
                OnPropertyChanged(nameof(SelectedMetric));

            }
        }
        public ICommand LoadMetricCommand { get; }
        public ICommand AddMetricsCommand { get; }
        public MetricReportViewModel(int playerId,MetricDataStore metricDataStore, NavigationStore navigationStore)
        {
            PlayerId = playerId;
            _metricDataStore = metricDataStore;
            _navigationStore = navigationStore;
            _metricListItemViewModels = new ObservableCollection<MetricListItemViewModel>();
            _metricDataStore.Loaded += _metricDataStore_Loaded;
            _metricDataStore.Created += _metricDataStore_Created;
            _metricDataStore.Updated += _metricDataStore_Updated;
            _metricDataStore.Deleted += _metricDataStore_Deleted;
            LoadMetricCommand = new LoadMetricsCommand(this, _metricDataStore);
            AddMetricsCommand = new NavaigateCommand<AddMetricsViewModel>(new NavigationService<AddMetricsViewModel>(_navigationStore, () => new AddMetricsViewModel(PlayerId,_metricDataStore, _navigationStore, this)));
            LoadMetricCommand.Execute(PlayerId);

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
            MetricListItemViewModel viewmodel = new(metric, _metricDataStore, _navigationStore, this);
            _metricListItemViewModels.Add(viewmodel);
        }

    }
}
