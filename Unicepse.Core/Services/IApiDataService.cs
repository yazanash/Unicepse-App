using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Core.Services
{
    public interface IApiDataService<T>
    {
        Task<int> Create(T entity);
        Task<int> Update(T entity);
        Task<T> Get(T entity);
    }
}
