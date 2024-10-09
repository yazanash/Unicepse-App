using Unicepse.Core.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Core.Services
{
    public interface IAccountDataService<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(int id);
        bool HasUsers();
        public Task<T> GetByUsername(string username);
        public Task<IEnumerable<AuthenticationLog>> GetAllAuthenticationLogging(DateTime date);
        public void AuthenticationLogging(AuthenticationLog entity);
    }
}
