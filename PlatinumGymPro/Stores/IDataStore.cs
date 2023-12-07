using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
    public interface IDataStore<T>
    {
        public event Action<T>? Created;
        public event Action? Loaded;
        public event Action<T>? Updated;
        public event Action<int>? Deleted;
        public  Task GetAll();
        public  Task Add(T entity);
        public  Task Update(T entity);

        public Task Delete(int entity_id);
        public Task Initialize();

    }
}
