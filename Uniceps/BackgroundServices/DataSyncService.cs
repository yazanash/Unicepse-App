using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Uniceps.API;
using Uniceps.API.Services;
using Microsoft.Extensions.DependencyInjection;
namespace Uniceps.BackgroundServices
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
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DataSyncService> _logger;

        public DataSyncService(
            BackgroundServiceStore backgroundServiceStore,
            IServiceScopeFactory scopeFactory,
            ILogger<DataSyncService> logger)
        {
            _backgroundServiceStore = backgroundServiceStore;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                int intervalSeconds = SyncSettingsManager.Current.SyncIntervalSeconds >= 3 ? SyncSettingsManager.Current.SyncIntervalSeconds: 10;

                if (SyncSettingsManager.Current.AutoSyncEnabled)
                {
                    try
                    {
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var apiClient = scope.ServiceProvider.GetRequiredService<UnicepseApiPrepHttpClient>();

                            bool apiConnected = await apiClient.CheckConnectionAsync();

                            if (apiConnected)
                            {
                                _backgroundServiceStore.ChangeState("تم الاتصال", true);
                                _backgroundServiceStore.SyncState(true, "جارِ المزامنة...");

                                await _backgroundServiceStore.Sync(stoppingToken);

                                _backgroundServiceStore.SyncState(true, "تمت المزامنة بنجاح");
                            }
                            else
                            {
                                _backgroundServiceStore.ChangeState("غير متصل", false);
                                _backgroundServiceStore.SyncState(false, "فشل الاتصال بالمخدم");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "فشلت المزامنة بسبب خطأ: {Message}", ex.Message);
                        _backgroundServiceStore.SyncState(false, "حدثت مشكلة أثناء المزامنة، ستتم المحاولة لاحقاً");
                    }
                }
                else
                {
                    _backgroundServiceStore.ChangeState("المزامنة التلقائية غير مفعلة", false);
                }

                await Task.Delay(TimeSpan.FromSeconds(intervalSeconds), stoppingToken);
            }
        }
    }
}
