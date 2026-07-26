using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Stores;

namespace Uniceps.Commands.SubscriptionCommand
{
    public class LoadTrainersItemCommand:AsyncCommandBase
    {
        private readonly EmployeeStore _store;

        public LoadTrainersItemCommand(EmployeeStore store)
        {
            _store = store;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
          await _store.GetAll();
        }
    }
}
