using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Player;
using Unicepse.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.API.Services;
using Unicepse.Core.Common;
using Unicepse.BackgroundServices;

namespace Unicepse.Stores
{
    public class PaymentDataStore
    {
        public event Action<PlayerPayment>? Created;
        public event Action? Loaded;
        public event Action<PlayerPayment>? Updated;
        public event Action<int>? Deleted;
        public event Action? SumUpdated;
        public PaymentDataStore(PaymentDataService paymentDataService, DausesDataService dausesDataService, PaymentApiDataService paymentApiDataService)
        {
            _paymentDataService = paymentDataService;
            _payments = new List<PlayerPayment>();
            _dausesDataService = dausesDataService;
            _paymentApiDataService = paymentApiDataService;
        }

        private readonly PaymentDataService _paymentDataService;
        private readonly PaymentApiDataService _paymentApiDataService;
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
            entity.DataStatus = DataStatus.ToCreate;
            await _paymentDataService.Create(entity);
            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            if (internetAvailable)
            {
                try
                {
                    int status = await _paymentApiDataService.Create(entity);
                    if (status == 201 || status == 409)
                    {
                        entity.DataStatus = DataStatus.Synced;
                        await _paymentDataService.Update(entity);
                    }
                }
                catch { }

            }
            _payments.Add(entity);
            Created?.Invoke(entity);
            SumUpdated?.Invoke();
        }


        public async Task Delete(PlayerPayment entity)
        {
            if (entity.DataStatus == DataStatus.ToCreate)
                await DeleteForce(entity.Id);
            else
            {
                bool internetAvailable = InternetAvailability.IsInternetAvailable();
                if (internetAvailable)
                {
                    try
                    {
                        int status = await _paymentApiDataService.Delete(entity);
                        if (status == 200 )
                        {
                            await DeleteForce(entity.Id);
                        }
                        else
                        {
                            entity.DataStatus = DataStatus.ToDelete;
                            await _paymentDataService.Update(entity);
                            int currentIndex = _payments.FindIndex(y => y.Id == entity.Id);
                            _payments.RemoveAt(currentIndex);
                        }
                    }
                    catch
                    {
                        entity.DataStatus = DataStatus.ToDelete;
                        await _paymentDataService.Update(entity);
                        int currentIndex = _payments.FindIndex(y => y.Id == entity.Id);
                        _payments.RemoveAt(currentIndex);
                    }

                }
                else
                {
                    entity.DataStatus = DataStatus.ToDelete;
                    await _paymentDataService.Update(entity);
                    int currentIndex = _payments.FindIndex(y => y.Id == entity.Id);
                    _payments.RemoveAt(currentIndex);
                }
             
            }
            Deleted?.Invoke(entity.Id);
            SumUpdated?.Invoke();
        }

        public async Task DeleteForce(int entity_id)
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
        public async Task GetAll(DateTime dateFrom, DateTime dateTo)
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
            if (entity.DataStatus != DataStatus.ToCreate)
                entity.DataStatus = DataStatus.ToUpdate;

            await _paymentDataService.Update(entity);
            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            if (internetAvailable)
            {
                try
                {


                    int status = await _paymentApiDataService.Update(entity);
                    if (status == 200)
                    {
                        entity.DataStatus = DataStatus.Synced;
                        await _paymentDataService.Update(entity);
                    }
                }
                catch { }
            }
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

        public async Task SyncPaymentsToCreate()
        {
            IEnumerable<PlayerPayment> payments = await _paymentDataService.GetByDataStatus(DataStatus.ToCreate);
            foreach (PlayerPayment payment in payments)
            {
                int status = await _paymentApiDataService.Create(payment);
                if (status==201||status==409)
                {
                    payment.DataStatus = DataStatus.Synced;
                    await _paymentDataService.Update(payment);
                }
            }
        }

        public async Task SyncPaymentsToUpdate()
        {
            IEnumerable<PlayerPayment> payments = await _paymentDataService.GetByDataStatus(DataStatus.ToUpdate);
            foreach (PlayerPayment payment in payments)
            {
                int status = await _paymentApiDataService.Update(payment);
                if (status==200)
                {
                    payment.DataStatus = DataStatus.Synced;
                    await _paymentDataService.Update(payment);
                }
            }
        }
        public async Task SyncPaymentsToDelete()
        {
            IEnumerable<PlayerPayment> payments = await _paymentDataService.GetByDataStatus(DataStatus.ToDelete);
            foreach (PlayerPayment payment in payments)
            {
                int status = await _paymentApiDataService.Delete(payment);
                if (status==200)
                {
                    await _paymentDataService.Delete(payment.Id);
                }


            }
        }
    }
}
