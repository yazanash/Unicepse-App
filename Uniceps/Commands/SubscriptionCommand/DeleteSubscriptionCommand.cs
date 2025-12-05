using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Subscription;
using Uniceps.Stores;

namespace Uniceps.Commands.SubscriptionCommand
{
    public class DeleteSubscriptionCommand:AsyncCommandBase
    {
        private readonly SubscriptionDataStore _subscriptionDataStore;

        public DeleteSubscriptionCommand(SubscriptionDataStore subscriptionDataStore)
        {
            _subscriptionDataStore = subscriptionDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if(parameter != null)
            {
                int subscriptionId = Convert.ToInt32(parameter);
                if(subscriptionId != 0)
                {
                   if( MessageBox.Show("سيتم حذف هذا الاشتراك بالاضافة لجميع الدفعات الخاصة به .... هل انت متاكد","تنويه",MessageBoxButton.YesNo,MessageBoxImage.Warning) ==
                        MessageBoxResult.Yes)
                    {
                        await _subscriptionDataStore.Delete(subscriptionId);
                    }

                }
            }
        }
    }
}
