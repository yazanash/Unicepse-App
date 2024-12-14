using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API.Models;
using Unicepse.Core.Models.Subscription;
using Unicepse.Core.Services;

namespace Unicepse.API.Services
{
    public class SubscriptionApiDataService : IApiDataService<SubscriptionDto>
    {
        private readonly UnicepseApiPrepHttpClient _client;

        public SubscriptionApiDataService(UnicepseApiPrepHttpClient client)
        {
            _client = client;
        }

        public async Task<int> Create(SubscriptionDto entity)
        {
            //SubscriptionDto subscriptionDto = new SubscriptionDto();
            //subscriptionDto.FromSubscription(entity);
            entity.gym_id = _client.id;
            return await _client.PostAsync("subscription", entity);
        }

        public async Task<SubscriptionDto> Get(SubscriptionDto entity)
        {
            SubscriptionDto subscriptionDto = await _client.GetAsync<SubscriptionDto>($"subscription/{_client.id}/{entity.pid!}/{entity.id}");
            return subscriptionDto;
        }
        public async Task<int> Update(SubscriptionDto entity)
        {
            //SubscriptionDto subscriptionDto = new SubscriptionDto();
            //subscriptionDto.FromSubscription(entity);
            entity.gym_id = _client.id;
            return await _client.PutAsync("subscription", entity);
        }
    }
}
