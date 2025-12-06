using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Uniceps.Core.Models.Authentication;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Services;

namespace Uniceps.Stores
{
    public class UsersDataStore : IDataStore<User>
    {
        private readonly IAccountDataService<User> _accountDataService;
        private readonly List<User> _users;

        public IEnumerable<User> Users => _users;

        string LogFlag = "[Users] ";
        private readonly ILogger<UsersDataStore> _logger;
        private readonly List<AuthenticationLog> _logs;

        public IEnumerable<AuthenticationLog> Logs => _logs;
        public UsersDataStore(IAccountDataService<User> accountDataService, ILogger<UsersDataStore> logger)
        {
            _accountDataService = accountDataService;
            _users = new List<User>();
            _logs = new();
            _logger = logger;
        }

        private User? _selectedUser;
        public User? SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;

            }
        }
        public event Action<User>? Created;
        public event Action? Loaded;
        public event Action? logs_Loaded;
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
            _logger.LogInformation(LogFlag + "add user");
            await _accountDataService.Create(entity);
            _users.Add(entity);
            Created?.Invoke(entity);
        }
        public void AddAuthenticationLog(AuthenticationLog entity)
        {
            _logger.LogInformation(LogFlag + "add logging");
            //_accountDataService.AuthenticationLogging(entity);
        }
        public async Task GetAuthenticationLog(DateTime date)
        {
            _logger.LogInformation(LogFlag + "get auth logs");
            IEnumerable<AuthenticationLog> logs = await _accountDataService.GetAllAuthenticationLogging(date);
            _logs.Clear();
            _logs.AddRange(logs);
            logs_Loaded?.Invoke();
        }
        public async Task Delete(int entity_id)
        {
            _logger.LogInformation(LogFlag + "delete user");
            bool deleted = await _accountDataService.Delete(entity_id);
            int currentIndex = _users.FindIndex(y => y.Id == entity_id);
            _users.RemoveAt(currentIndex);
            Deleted?.Invoke(entity_id);
        }

        public async Task GetAll()
        {
            _logger.LogInformation(LogFlag + "get all user");
            IEnumerable<User> employees = await _accountDataService.GetAll();
            _users.Clear();
            _users.AddRange(employees);
            Loaded?.Invoke();
        }

        public async Task Update(User entity)
        {
            _logger.LogInformation(LogFlag + "update user");
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
