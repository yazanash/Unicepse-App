using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Unicepse.API;

namespace Unicepse.BackgroundServices
{
    public class InternetAvailability
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }
    }
    public class DataSyncService : BackgroundService
    {
        private readonly BackgroundServiceStore _backgroundServiceStore;
        private readonly UnicepseApiPrepHttpClient _unicepseApiPrepHttpClient;
        public DataSyncService(BackgroundServiceStore backgroundServiceStore, UnicepseApiPrepHttpClient unicepseApiPrepHttpClient)
        {
            _backgroundServiceStore = backgroundServiceStore;
            _unicepseApiPrepHttpClient = unicepseApiPrepHttpClient;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                bool internetAvailable = InternetAvailability.IsInternetAvailable();
                if (internetAvailable)
                {
                    _backgroundServiceStore.ChangeState($"تم الاتصال", internetAvailable);
                    _backgroundServiceStore.SyncState(true,"جار مزامنة اللاعبين");
                    _backgroundServiceStore.ChangeState($"تم الاتصال", internetAvailable);
                    await _backgroundServiceStore.SyncPlayers();
                    _backgroundServiceStore.SyncState(true, "جار مزامنة الاشتراكات");
                    _backgroundServiceStore.ChangeState($"تم الاتصال", internetAvailable);
                    await _backgroundServiceStore.SyncSubscribtions();
                    _backgroundServiceStore.SyncState(true, "جار مزامنة المدفوعات");
                    _backgroundServiceStore.ChangeState($"تم الاتصال", internetAvailable);
                    await _backgroundServiceStore.SyncPayments();
                    _backgroundServiceStore.SyncState(true, "جار مزامنة القياسات");
                    _backgroundServiceStore.ChangeState($"تم الاتصال", internetAvailable);
                    await _backgroundServiceStore.SyncMetrics();
                    _backgroundServiceStore.SyncState(true, "جار مزامنة البرامج الرياضية");
                    _backgroundServiceStore.ChangeState($"تم الاتصال", internetAvailable);
                    await _backgroundServiceStore.SyncRoutines();
                    _backgroundServiceStore.SyncState(true, "جار مزامنة الحضور");
                    _backgroundServiceStore.ChangeState($"تم الاتصال", internetAvailable);
                    await _backgroundServiceStore.SyncAttendances();
                    _backgroundServiceStore.SyncState(false,"");
                }
                else
                    _backgroundServiceStore.ChangeState($"غير متصل", internetAvailable);
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
