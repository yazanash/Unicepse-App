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
        private readonly DausesDataService  _dausesDataService;
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
            await InsertDauses(entity);
            _payments.Add(entity);
            Created?.Invoke(entity);
            SumUpdated?.Invoke();
        }
        private async Task InsertDauses(PlayerPayment entity)
        {
            if (entity.Subscription!.IsMoved)
            {
                if (entity.Subscription!.LastPaid < entity.Subscription!.LastCheck)
                {
                    if (entity.Subscription!.PrevTrainer_Id != -1)
                    {
                        await InsertDausesForOldTrainer(entity);
                        if (entity.Subscription!.Trainer != null)
                        {
                            int oldDays = Convert.ToInt32(Math.Round((entity.Subscription!.LastCheck - entity.Subscription!.LastPaid).TotalDays));
                            await InsertDausesForNewTrainer(entity, oldDays);

                        }
                    }
                }
                else
                {
                    if (entity.Subscription!.Trainer != null)
                    {
                        await InsertDausesDefault(entity);
                    }
                }
            }
            else
            {
                if (entity.Subscription!.Trainer != null)
                {
                    await InsertDausesDefault(entity);
                }
            }
            //await InsertDausesDefault(entity);
        }
        private async Task InsertDausesDefault(PlayerPayment entity)
        {
            int sportDays = entity.Subscription!.DaysCount;
            double dayPrice = entity.Subscription!.PriceAfterOffer / sportDays;
            int daysCount = Convert.ToInt32(entity.PaymentValue / dayPrice);
            DateTime from = entity.Subscription!.LastPaid;
            TrainerDueses trainingDueses = new TrainerDueses
            {
                PlayerPayment = entity,
                PlayerTraining = entity.Subscription!,
                IsPrivate = entity.Subscription!.IsPrivate,
                From = from,
                To = entity.Subscription!.LastPaid.AddDays(daysCount),
                CreatedAt = entity.PayDate,
                Trainer = entity.Subscription!.Trainer,

            };
            trainingDueses.Value = entity.PaymentValue * (entity.Subscription!.Trainer!.ParcentValue / 100.0);
            //await _dausesDataService.Create(trainingDueses);
        }
        private async Task InsertDausesForOldTrainer(PlayerPayment entity)
        {
            int sportDays = entity.Subscription!.DaysCount;
          
            double dayPrice = entity.Subscription!.PriceAfterOffer / sportDays;
            int daysCount = Convert.ToInt32(entity.PaymentValue / dayPrice);
            DateTime from = entity.Subscription!.LastPaid;

            int prevDays = daysCount;
            Employee previous_trainer = await _paymentDataService.GetPreviousTrainer(entity.Subscription!.PrevTrainer_Id);
            TrainerDueses trainingDueses = new TrainerDueses
            {
                PlayerPayment = entity,
                PlayerTraining = entity.Subscription!,
                IsPrivate = entity.Subscription!.IsPrivate,
                From = from,
                CreatedAt = entity.PayDate,
                Trainer = previous_trainer,
            };


            double payVal = 0;
            if (entity.Subscription!.LastPaid.AddDays(daysCount) > entity.Subscription!.LastCheck && entity.Subscription!.LastPaid < entity.Subscription!.LastCheck)
            {
                int oldDays = Convert.ToInt32(Math.Round((entity.Subscription!.LastCheck - entity.Subscription!.LastPaid).TotalDays));
                prevDays = oldDays;
                payVal = dayPrice * oldDays;
                trainingDueses.To = trainingDueses.From.AddDays(oldDays);
                entity.Subscription!.LastPaid = entity.Subscription!.LastPaid.AddDays(oldDays);
            }
            else if (entity.Subscription!.LastPaid.AddDays(daysCount) <= entity.Subscription!.LastCheck)
            {
                payVal = entity.PaymentValue;
                trainingDueses.To = trainingDueses.From.AddDays(daysCount);
                entity.Subscription!.LastPaid = entity.Subscription!.LastPaid.AddDays(daysCount);
            }
            double value = (payVal * entity.Subscription!.Trainer!.ParcentValue) / 100.0;
            trainingDueses.Value = value;
            //await _dausesDataService.Create(trainingDueses);
        }
        private async Task InsertDausesForNewTrainer(PlayerPayment entity,int prevDays)
        {
            int sportDays = entity.Subscription!.DaysCount;
        

            double dayPrice = entity.Subscription!.PriceAfterOffer / sportDays;
            double daysCount = entity.PaymentValue / dayPrice;
            daysCount -= prevDays;
            DateTime from = entity.Subscription!.LastPaid;
            if (entity.Subscription!.LastPaid.AddDays(daysCount) > entity.Subscription!.LastCheck)
            {
                TrainerDueses trainingDueses = new TrainerDueses
                {
                    PlayerPayment = entity,
                    PlayerTraining = entity.Subscription!,
                    IsPrivate = entity.Subscription!.IsPrivate,
                    From = from,
                    CreatedAt = entity.PayDate,
                    Trainer = entity.Subscription!.Trainer,
                };


                double payVal = 0;

                int newDays = Convert.ToInt32((entity.Subscription!.LastPaid.AddDays(daysCount) - entity.Subscription!.LastCheck).TotalDays);
                payVal = dayPrice * newDays;
                trainingDueses.To = trainingDueses.From.AddDays(newDays);
                entity.Subscription!.LastPaid = entity.Subscription!.LastPaid.AddDays(newDays);

                double value = (payVal * entity.Subscription!.Trainer!.ParcentValue) / 100;
                trainingDueses.Value = value;
                //await _dausesDataService.Create(trainingDueses);

            }
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
        public Task Initialize()
        {
            throw new NotImplementedException();
        }
        public double GetSum()
        {
           double sum = Payments.Sum(x=>x.PaymentValue);
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
