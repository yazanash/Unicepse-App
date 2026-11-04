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
using Uniceps.Core.Models.Metric;
using Uniceps.navigation.NavigationSystem;
using Uniceps.navigation.Extensions;
using Uniceps.navigation.DialogService;
using Uniceps.Views.Metrics;

namespace Uniceps.ViewModels.Metrics
{
    public class MetricReportViewModel : ListingViewModelBase,IQueryAttributable
    {
        private readonly IDialogService _dialogService;
        private readonly MetricDataStore _metricDataStore;
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
        public MetricReportViewModel(MetricDataStore metricDataStore,  IDialogService dialogService)
        {
            _metricDataStore = metricDataStore;
            _dialogService = dialogService;
            _metricListItemViewModels = new ObservableCollection<MetricListItemViewModel>();
            _metricDataStore.Loaded += _metricDataStore_Loaded;
            _metricDataStore.Created += _metricDataStore_Created;
            _metricDataStore.Updated += _metricDataStore_Updated;
            _metricDataStore.Deleted += _metricDataStore_Deleted;
            LoadMetricCommand = new LoadMetricsCommand(this, _metricDataStore);
           
            AddMetricsCommand = new RelayCommand(ExecuteAddMetricCommand);
         
        }

        private void ExecuteAddMetricCommand()
        {
            var navParams = new Dictionary<string, object>
            {
                {"PlayerId",PlayerId},

            };
            _dialogService.ShowDialog<AddMetricWindowView, AddMetricsViewModel>(navParams);
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
            MetricListItemViewModel viewmodel = new(metric,_dialogService);
            _metricListItemViewModels.Add(viewmodel);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue<int>("PlayerId", out var playerId))
            {
                PlayerId = playerId;
                LoadMetricCommand.Execute(PlayerId);
            }

        }
    }
}
