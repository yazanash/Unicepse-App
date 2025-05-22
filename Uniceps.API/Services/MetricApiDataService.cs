using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.Models;
using Uniceps.Core.Services;
using Uniceps.Core.Models.Metric;

namespace Uniceps.API.Services
{
    public class MetricApiDataService : IApiDataService<MetricDto>
    {
        private readonly UnicepseApiPrepHttpClient _client;

        public MetricApiDataService(UnicepseApiPrepHttpClient client)
        {
            _client = client;
        }

        public async Task<int> Create(MetricDto entity)
        {
            //MetricDto metricDto = new MetricDto();
            //metricDto.FromMetric(entity);
            entity.gym_id = _client.id;
            return await _client.PostAsync("metrics", entity);
        }

        public async Task<MetricDto> Get(MetricDto entity)
        {
            MetricDto metricDto = await _client.GetAsync<MetricDto>($"metrics/{_client.id}/{entity.pid}/{entity.id}");
            return metricDto;
        }

        public async Task<int> Update(MetricDto entity)
        {
            //MetricDto metricDto = new MetricDto();
            //metricDto.FromMetric(entity);
            entity.gym_id = _client.id;
            return await _client.PutAsync("metrics", entity);
        }
    }
}
