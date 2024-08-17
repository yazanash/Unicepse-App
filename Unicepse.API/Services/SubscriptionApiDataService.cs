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
    public class SubscriptionApiDataService : IApiDataService<Subscription>
    {
        private readonly UnicepseApiPrepHttpClient _client;

        public SubscriptionApiDataService(UnicepseApiPrepHttpClient client)
        {
            _client = client;
        }

        public async Task<bool> Create(Subscription entity)
        {
            SubscriptionDto subscriptionDto = new SubscriptionDto();
            subscriptionDto.FromSubscription(entity);
            subscriptionDto.gym_id = 18;
            return await _client.PostAsync("subscription", subscriptionDto);
        }

        public async Task<Subscription> Get(Subscription entity)
        {
            SubscriptionDto subscriptionDto = await _client.GetAsync<SubscriptionDto>($"subscription/18/{entity.Player!.Id}/{entity.Id}");
            return subscriptionDto.ToSubscription();
        }
        public async Task<bool> Update(Subscription entity)
        {
            SubscriptionDto subscriptionDto = new SubscriptionDto();
            subscriptionDto.FromSubscription(entity);
            subscriptionDto.gym_id = 18;
            return await _client.PutAsync("subscription", subscriptionDto);
        }
    }
}
