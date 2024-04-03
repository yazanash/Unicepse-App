using PlatinumGym.Core.Models.Authentication;
using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Stores
{
    public class UsersDataStore : IDataStore<User>
    {
        private readonly IAccountDataService<User> _accountDataService;
        private readonly List<User> _users;

        public IEnumerable<User> Users => _users;
        public UsersDataStore(IAccountDataService<User> accountDataService)
        {
            _accountDataService = accountDataService;
            _users = new List<User>();

        }
        public event Action<User>? Created;
        public event Action? Loaded;
        public event Action<User>? Updated;
        public event Action<int>? Deleted;
        private Employee? _selectedEmployee;
        public Employee? SelectedEmployee
        {
            get
            {
                return _selectedEmployee;
            }
            set
            {
                _selectedEmployee = value;
                
            }
        }
        public async Task Add(User entity)
        {
            await _accountDataService.Create(entity);
            _users.Add(entity);
            Created?.Invoke(entity);
        }

        public async Task Delete(int entity_id)
        {
            bool deleted = await _accountDataService.Delete(entity_id);
            int currentIndex = _users.FindIndex(y => y.Id == entity_id);
            _users.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll()
        {
            IEnumerable<User> employees = await _accountDataService.GetAll();
            _users.Clear();
            _users.AddRange(employees);
            Loaded?.Invoke();
        }

        public async Task Update(User entity)
        {
            await _accountDataService.Update(entity);
            int currentIndex = _users.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _users[currentIndex] = entity;
            }
            else
            {
                _users.Add(entity);
            }
            Updated?.Invoke(entity);
        }

        public Task Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
