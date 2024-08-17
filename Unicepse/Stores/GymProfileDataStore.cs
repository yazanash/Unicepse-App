using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models;
using Unicepse.Entityframework.Services;

namespace Unicepse.Stores
{
    public class GymProfileDataStore : IDataStore<GymProfile>
    {
        public event Action<GymProfile>? Created;
        public event Action? Loaded;
        public event Action<GymProfile>? Updated;
        public event Action<int>? Deleted;
        private readonly GymProfileDataService _gymProfileDataService;
        public GymProfileDataStore(GymProfileDataService gymProfileDataService)
        {
            _gymProfileDataService = gymProfileDataService;
        }

        public Task Add(GymProfile entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int entity_id)
        {
            throw new NotImplementedException();
        }

        public Task GetAll()
        {
            throw new NotImplementedException();
        }

        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public Task Update(GymProfile entity)
        {
            throw new NotImplementedException();
        }
    }
}
