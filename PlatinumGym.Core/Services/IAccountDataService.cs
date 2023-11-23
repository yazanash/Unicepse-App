using PlatinumGym.Core.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Core.Services
{
    public interface IAccountDataService<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(int id);
        public Task<T> GetByUsername(string username);
    }
}
