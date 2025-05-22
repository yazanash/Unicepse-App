using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API;
using Uniceps.API.Models;
using Uniceps.Core.Services;
using Uniceps.Core.Models.Subscription;

namespace Uniceps.API.Services
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
