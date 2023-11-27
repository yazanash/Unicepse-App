using PlatinumGym.Core.Models.Sport;
using PlatinumGym.Entityframework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
    public class SportDataStore : IDataStore<Sport>
    {
        public event Action<Sport>? Created;
        public event Action<IEnumerable<Sport>>? Loaded;
        public event Action<Sport>? Updated;
        public event Action<bool>? Deleted;

        private readonly SportServices _sportDataService;
        private readonly List<Sport> _sports;
        private readonly Lazy<Task> _initializeLazy;

        public IEnumerable<Sport> Sports => _sports;
       
        public SportDataStore(SportServices sportDataService)
        {
            _sportDataService = sportDataService;
            _sports = new List<Sport>();
            _initializeLazy = new Lazy<Task>(Initialize);
        }

        public async Task Add(Sport entity)
        {
            await _sportDataService.Create(entity);
            _sports.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Delete(int entity_id)
        {
            bool deleted = await _sportDataService.Delete(entity_id);
            int currentIndex = _sports.FindIndex(y => y.Id == entity_id);
            _sports.RemoveAt(currentIndex);
            Deleted?.Invoke(deleted);
        }

        public async Task GetAll()
        {
            await _initializeLazy.Value;
        }

        public async Task Initialize()
        {
            IEnumerable<Sport> sports = await _sportDataService.GetAll();
            _sports.Clear();
            _sports.AddRange(sports);
        }

        public async Task Update(Sport entity)
        {
            await _sportDataService.Update(entity);
            int currentIndex = _sports.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _sports[currentIndex] = entity;
            }
            else
            {
                _sports.Add(entity);
            }
            Updated?.Invoke(entity);
        }
    }
}
