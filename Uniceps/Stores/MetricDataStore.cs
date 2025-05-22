using Uniceps.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.Services;
using Uniceps.BackgroundServices;
using Microsoft.Extensions.Logging;
using Uniceps.Stores.ApiDataStores;
using Uniceps.Core.Services;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Metric;
using Uniceps.Core.Common;

namespace Uniceps.Stores
{
    public class MetricDataStore : IDataStore<Metric>
    {
        private readonly IDataService<Metric> _metricDataService;
        private readonly IApiDataStore<Metric> _apiDataStore;
        private readonly IGetPlayerTransactionService<Metric> _getPlayerTransactionService;
        private readonly List<Metric> _metrics;
        string LogFlag = "[Metrics] ";
        private readonly ILogger<MetricDataStore> _logger;
        public IEnumerable<Metric> Metrics => _metrics;

        public event Action<Metric>? Created;
        public event Action? Loaded;
        public event Action<Metric>? Updated;
        public event Action<int>? Deleted;
        public MetricDataStore(IDataService<Metric> metricDataService, ILogger<MetricDataStore> logger, IGetPlayerTransactionService<Metric> getPlayerTransactionService, IApiDataStore<Metric> apiDataStore)
        {
            _metricDataService = metricDataService;
            _metrics = new List<Metric>();
            _logger = logger;
            _getPlayerTransactionService = getPlayerTransactionService;
            _apiDataStore = apiDataStore;
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
                _logger.LogInformation(LogFlag + "selected metrics changed");
                StateChanged?.Invoke(SelectedMetric);
            }
        }
        public event Action<Metric?>? StateChanged;
        public async Task Add(Metric entity)
        {
            _logger.LogInformation(LogFlag + "add metrics");
            entity.DataStatus = DataStatus.ToCreate;
            await _metricDataService.Create(entity);
            await _apiDataStore.Create(entity);
            _metrics.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Delete(int entity_id)
        {
            _logger.LogInformation(LogFlag + "delete metrics");
            bool deleted = await _metricDataService.Delete(entity_id);
            int currentIndex = _metrics.FindIndex(y => y.Id == entity_id);
            _metrics.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll()
        {
            _logger.LogInformation(LogFlag + "get all metrics");
            IEnumerable<Metric> subscriptions = await _metricDataService.GetAll();
            _metrics.Clear();
            _metrics.AddRange(subscriptions);
            Loaded?.Invoke();
        }
        public async Task GetAll(Player player)
        {
            _logger.LogInformation(LogFlag + "get all player metrics");
            IEnumerable<Metric> metric = await _getPlayerTransactionService.GetAll(player);
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
            _logger.LogInformation(LogFlag + "update metrics");
            if (entity.DataStatus != DataStatus.ToCreate)
                entity.DataStatus = DataStatus.ToUpdate;

            await _metricDataService.Update(entity);
            await _apiDataStore.Update(entity);

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
