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

        public async Task<bool> Create(Metric entity)
        {
            MetricDto metricDto = new MetricDto();
            metricDto.FromMetric(entity);
            metricDto.gym_id = 18;
            return await _client.PostAsync("metrics", metricDto);
        }

        public async Task<Metric> Get(Metric entity)
        {
            MetricDto metricDto = await _client.GetAsync<MetricDto>($"metrics/18/{entity.Player!.Id}/{entity.Id}");
            return metricDto.ToMetric();
        }

        public async Task<bool> Update(Metric entity)
        {
            MetricDto metricDto = new MetricDto();
            metricDto.FromMetric(entity);
            metricDto.gym_id = 18;
            return await _client.PutAsync("metrics", metricDto);
        }
    }
}
