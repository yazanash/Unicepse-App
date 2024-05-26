using Unicepse.Core.Models.Metric;
using Unicepse.Core.Models.Player;
using Unicepse.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.WPF.Stores
{
    public class MetricDataStore : IDataStore<Metric>
    {
        private readonly MetricDataService _metricDataService;
        private readonly List<Metric> _metrics;
        public IEnumerable<Metric> Metrics => _metrics;

        public event Action<Metric>? Created;
        public event Action? Loaded;
        public event Action<Metric>? Updated;
        public event Action<int>? Deleted;
        public MetricDataStore(MetricDataService metricDataService)
        {
            _metricDataService = metricDataService;
            _metrics = new List<Metric>();
        }
        private Metric? _selectedMetric;
        public Metric? SelectedMetric
        {
            get
            {
                return _selectedMetric;
            }
            set
            {
                _selectedMetric = value;
                StateChanged?.Invoke(SelectedMetric);
            }
        }
        public event Action<Metric?>? StateChanged;
        public async Task Add(Metric entity)
        {
            await _metricDataService.Create(entity);
            _metrics.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Delete(int entity_id)
        {
            bool deleted = await _metricDataService.Delete(entity_id);
            int currentIndex = _metrics.FindIndex(y => y.Id == entity_id);
            _metrics.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll()
        {
            IEnumerable<Metric> subscriptions = await _metricDataService.GetAll();
            _metrics.Clear();
            _metrics.AddRange(subscriptions);
            Loaded?.Invoke();
        }
        public async Task GetAll(Player player)
        {
            IEnumerable<Metric> metric = await _metricDataService.GetAll(player);
            _metrics.Clear();
            _metrics.AddRange(metric);
            Loaded?.Invoke();
        }

        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public async Task Update(Metric entity)
        {
            await _metricDataService.Update(entity);
            int currentIndex = _metrics.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _metrics[currentIndex] = entity;
            }
            else
            {
                _metrics.Add(entity);
            }
            Updated?.Invoke(entity);
        }
    }
}
