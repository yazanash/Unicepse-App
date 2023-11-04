using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
    public class TrainerStore
    {
        private readonly GenericDataService<Employee> TrainerService;
        private readonly List<Employee> _trainers;
        public IEnumerable<Employee> Trainer => _trainers;


        public event Action? TrainersLoaded;
        public event Action<Employee>? TrainerAdded;
        public event Action<Employee>? TrainerUpdated;
        public event Action<int>? TrainerDeleted;
        public TrainerStore(GenericDataService<Employee> trainerService)
        {
            TrainerService = trainerService;
            _trainers = new List<Employee>();
        }
        public async Task Load()
        {
            IEnumerable<Employee> trainers = await TrainerService.GetAll();
            
            _trainers.Clear();
            _trainers.AddRange(trainers);

            TrainersLoaded?.Invoke();
        }
        public async Task Add(Employee trainer)
        {
            await TrainerService.Create(trainer);

            _trainers.Add(trainer);

            TrainerAdded?.Invoke(trainer);
        }
        public async Task Update(Employee trainer)
        {
            await TrainerService.Update(trainer);

            int currentIndex = _trainers.FindIndex(y => y.Id == trainer.Id);

            if (currentIndex != -1)
            {
                _trainers[currentIndex] = trainer;
            }
            else
            {
                _trainers.Add(trainer);
            }

            TrainerUpdated?.Invoke(trainer);
        }
        public async Task Delete(int id)
        {
            await TrainerService.Delete(id);

            _trainers.RemoveAll(y => y.Id == id);

            TrainerDeleted?.Invoke(id);
        }
    }
}
