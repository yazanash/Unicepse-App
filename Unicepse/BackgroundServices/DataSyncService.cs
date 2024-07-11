﻿using Microsoft.Extensions.Hosting;
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
        public DataSyncService(BackgroundServiceStore backgroundServiceStore)
        {
            _backgroundServiceStore = backgroundServiceStore;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Your background logic here
                bool internetAvailable = InternetAvailability.IsInternetAvailable();
                if (internetAvailable)
                    _backgroundServiceStore.ChangeState($"تم الاتصال", internetAvailable);
                else
                    _backgroundServiceStore.ChangeState($"غير متصل", internetAvailable);

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
