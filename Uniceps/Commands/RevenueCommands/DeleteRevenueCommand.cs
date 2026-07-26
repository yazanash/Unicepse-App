using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Stores;

namespace Uniceps.Commands.RevenueCommands
{
    public class DeleteRevenueCommand : AsyncCommandBase
    {
        private readonly OtherRevenuesDataStore _otherRevenuesDataStore;

        public DeleteRevenueCommand(OtherRevenuesDataStore otherRevenuesDataStore)
        {
            _otherRevenuesDataStore = otherRevenuesDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (parameter != null && parameter is int id)
                await _otherRevenuesDataStore.Delete(id);
        }
    }
}
