using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API.Models;
using Unicepse.Core.Models.Metric;
using Unicepse.Core.Services;

namespace Unicepse.API.Services
{
    public class MetricApiDataService : IApiDataService<Metric>
    {
        private readonly UnicepseApiPrepHttpClient _client;

        public MetricApiDataService(UnicepseApiPrepHttpClient client)
        {
            _client = client;
        }

        public async Task<int> Create(Metric entity)
        {
            MetricDto metricDto = new MetricDto();
            metricDto.FromMetric(entity);
            metricDto.gym_id = _client.id;
            return await _client.PostAsync("metrics", metricDto);
        }

        public async Task<Metric> Get(Metric entity)
        {
            MetricDto metricDto = await _client.GetAsync<MetricDto>($"metrics/{_client.id}/{entity.Player!.Id}/{entity.Id}");
            return metricDto.ToMetric();
        }

        public async Task<int> Update(Metric entity)
        {
            MetricDto metricDto = new MetricDto();
            metricDto.FromMetric(entity);
            metricDto.gym_id = _client.id;
            return await _client.PutAsync("metrics", metricDto);
        }
    }
}
