using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.SyncModel;

namespace Unicepse.Stores.ApiDataStores
{
    public interface IApiDataStore<T>
    {
        public Task Create(T entity);
        public Task Update(T entity);
        public Task Sync(SyncObject syncObject);
    }
}
