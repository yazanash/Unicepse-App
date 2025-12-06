using Uniceps.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;

namespace Uniceps.Core.Services
{
    public interface IDataService<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(int id);
    }
}
