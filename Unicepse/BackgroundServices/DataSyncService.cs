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
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<DataSyncService> _logger;
        string LogFlag = "[Background service] ";
        public DataSyncService(BackgroundServiceStore backgroundServiceStore, UnicepseApiPrepHttpClient unicepseApiPrepHttpClient, ILogger<DataSyncService> logger)
        {
            _backgroundServiceStore = backgroundServiceStore;
            _unicepseApiPrepHttpClient = unicepseApiPrepHttpClient;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                bool internetAvailable = InternetAvailability.IsInternetAvailable();
                if (internetAvailable)
                {
                    _logger.LogInformation(LogFlag+"Check internet connection {0}",internetAvailable.ToString());
                    try
                    {
                        _backgroundServiceStore.ChangeState($"تم الاتصال", internetAvailable);
                        _backgroundServiceStore.SyncState(true, "جار مزامنة");
                        _backgroundServiceStore.ChangeState($"تم الاتصال", internetAvailable);
                        await _backgroundServiceStore.Sync();
                        _backgroundServiceStore.SyncState(false, "");
                        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                    }
                   catch(Exception ex)
                    {
                        _logger.LogError(LogFlag + "failed to sync with error {0}",ex.Message);
                        _backgroundServiceStore.SyncState(true, "حدثت مشكلة اثناء المزامنة ستتم المحاولة خلال 10 ثوان");
                        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

                    }
                }
                else
                    _backgroundServiceStore.ChangeState($"غير متصل", internetAvailable);
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
