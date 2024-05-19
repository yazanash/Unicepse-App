using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Models.Payment;
using PlatinumGym.Core.Models.Player;
using PlatinumGym.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Stores
{
    public class PaymentDataStore : IDataStore<PlayerPayment>
    {
        public event Action<PlayerPayment>? Created;
        public event Action? Loaded;
        public event Action<PlayerPayment>? Updated;
        public event Action<int>? Deleted;
        public event Action? SumUpdated;
        public PaymentDataStore(PaymentDataService paymentDataService, DausesDataService dausesDataService)
        {
            _paymentDataService = paymentDataService;
            _payments = new List<PlayerPayment>();
            _dausesDataService = dausesDataService;
        }

        private readonly PaymentDataService _paymentDataService;
        private readonly DausesDataService _dausesDataService;
        private readonly List<PlayerPayment> _payments;
        public IEnumerable<PlayerPayment> Payments => _payments;




        private PlayerPayment? _selectedPayment;
        public PlayerPayment? SelectedPayment
        {
            get
            {
                return _selectedPayment;
            }
            set
            {
                _selectedPayment = value;
            }
        }




        public async Task Add(PlayerPayment entity)
        {
            await _paymentDataService.Create(entity);
            _payments.Add(entity);
            Created?.Invoke(entity);
            SumUpdated?.Invoke();
        }
       

        public async Task Delete(int entity_id)
        {
            bool deleted = await _paymentDataService.Delete(entity_id);
            int currentIndex = _payments.FindIndex(y => y.Id == entity_id);
            _payments.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
            SumUpdated?.Invoke();
        }

        public async Task GetPlayerPayments(Player player)
        {
            IEnumerable<PlayerPayment> subscriptions = await _paymentDataService.GetPlayerPayments(player);
            _payments.Clear();
            _payments.AddRange(subscriptions);
            Loaded?.Invoke();
            SumUpdated?.Invoke();
        }
        public async Task GetAll()
        {
            IEnumerable<PlayerPayment> subscriptions = await _paymentDataService.GetAll();
            _payments.Clear();
            _payments.AddRange(subscriptions);
            Loaded?.Invoke();
            SumUpdated?.Invoke();
        }
        public async Task GetAll(DateTime dateFrom,DateTime dateTo)
        {
            IEnumerable<PlayerPayment> subscriptions = await _paymentDataService.GetAll(dateFrom, dateTo);
            _payments.Clear();
            _payments.AddRange(subscriptions);
            Loaded?.Invoke();
            SumUpdated?.Invoke();
        }
        public Task Initialize()
        {
            throw new NotImplementedException();
        }
        public double GetSum()
        {
            double sum = Payments.Sum(x => x.PaymentValue);
            return sum;
        }
        public async Task Update(PlayerPayment entity)
        {
            await _paymentDataService.Update(entity);
            int currentIndex = _payments.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _payments[currentIndex] = entity;
            }
            else
            {
                _payments.Add(entity);
            }
            Updated?.Invoke(entity);
            SumUpdated?.Invoke();
        }
    }
}
