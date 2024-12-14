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
    public class PaymentApiDataService : IApiDataService<PaymentDto>
    {
        private readonly UnicepseApiPrepHttpClient _client;

        public PaymentApiDataService(UnicepseApiPrepHttpClient client)
        {
            _client = client;
        }

        public async Task<int> Create(PaymentDto entity)
        {
            //PaymentDto paymentDto = new PaymentDto();
            //paymentDto.FromPayment(entity);
            entity.gym_id = _client.id;
            return await _client.PostAsync("payments", entity);
        }

        public async Task<PaymentDto> Get(PaymentDto entity)
        {
            PaymentDto paymentDto = await _client.GetAsync<PaymentDto>($"payments/{_client.id}/{entity.pid}/{entity.sid}/{entity.id}");
            return paymentDto;
        }
        public async Task<int> Update(PaymentDto entity)
        {
            //PaymentDto paymentDto = new PaymentDto();
            //paymentDto.FromPayment(entity);
            entity.gym_id = _client.id;
            return await _client.PutAsync("payments", entity);
        }
        public async Task<int> Delete(PaymentDto entity)
        {
            //PaymentDto paymentDto = new PaymentDto();
            //paymentDto.FromPayment(entity);
            entity.gym_id = _client.id;
            return await _client.DeleteAsync<bool>($"payments/{entity.gym_id}/{entity.pid}/{entity.sid}/{entity.id}");
        }
    }
}
