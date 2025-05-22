using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Stores.ApiDataStores
{
    public interface IDeleteApiDataStore<T>
    {
        public Task Delete(T entity);
    }
}
