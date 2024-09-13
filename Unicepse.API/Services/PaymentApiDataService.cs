using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API.Models;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Services;

namespace Unicepse.API.Services
{
    public class PaymentApiDataService : IApiDataService<PlayerPayment>
    {
        private readonly UnicepseApiPrepHttpClient _client;

        public PaymentApiDataService(UnicepseApiPrepHttpClient client)
        {
            _client = client;
        }

        public async Task<int> Create(PlayerPayment entity)
        {
            PaymentDto paymentDto = new PaymentDto();
            paymentDto.FromPayment(entity);
            paymentDto.gym_id = _client.id;
            return await _client.PostAsync("payments", paymentDto);
        }

        public async Task<PlayerPayment> Get(PlayerPayment entity)
        {
            PaymentDto paymentDto = await _client.GetAsync<PaymentDto>($"payments/{_client.id}/{entity.Player!.Id}/{entity.Subscription!.Id}/{entity.Id}");
            return paymentDto.ToPayment();
        }
        public async Task<int> Update(PlayerPayment entity)
        {
            PaymentDto paymentDto = new PaymentDto();
            paymentDto.FromPayment(entity);
            paymentDto.gym_id = _client.id;
            return await _client.PutAsync("payments", paymentDto);
        }
        public async Task<int> Delete(PlayerPayment entity)
        {
            PaymentDto paymentDto = new PaymentDto();
            paymentDto.FromPayment(entity);
            paymentDto.gym_id = _client.id;
            return await _client.DeleteAsync<bool>($"payments/{paymentDto.gym_id}/{paymentDto.pid}/{paymentDto.sid}/{paymentDto.id}");
        }
    }
}
