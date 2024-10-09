using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;

namespace Unicepse.Commands.AccountantCommand
{
    public class LoadDailyCredits : AsyncCommandBase
    {
        private readonly GymStore _gymStore;
        public LoadDailyCredits(GymStore gymStore)
        {
            _gymStore = gymStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await _gymStore.GetDailyCredits(DateTime.Now);
        }
    }
}
