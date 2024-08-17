using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Core.Services
{
    public interface IApiDataService<T>
    {
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<T> Get(T entity);
    }
}
