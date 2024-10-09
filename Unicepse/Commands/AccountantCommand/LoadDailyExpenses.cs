using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;

namespace Unicepse.Commands.AccountantCommand
{
    public class LoadDailyExpenses : AsyncCommandBase
    {
        private readonly GymStore _gymStore;
        public LoadDailyExpenses(GymStore gymStore)
        {
            _gymStore = gymStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _gymStore.GetDailyExpenses(DateTime.Now);
        }
    }
}
