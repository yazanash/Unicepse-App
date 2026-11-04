using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.API;
using Uniceps.API.Models;
using Uniceps.API.Services;
using Uniceps.BackgroundServices;
using Uniceps.Entityframework.DbContexts;
using Uniceps.LicenseManager;
using Uniceps.SystemServices;

namespace Uniceps.HostBuilders
{
    public static class AddHttpClientHostBuilderExtentions
    {
        public static IHostBuilder AddHttpClient(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices((hostContext, services) =>
            {
                string apiUrl = hostContext.Configuration["HostSettings:BaseUrl"] ?? "";
                string publicKey = hostContext.Configuration["LicenseSettings:PublicKey"] ?? "";
                bool SQLITE = hostContext.Configuration.GetValue<bool>("UseSqlite");

                string CONNECTION_STRING = SQLITE
                    ? DatabaseStorageService.ResolveSQLitePath(hostContext.Configuration)
                    : hostContext.Configuration.GetConnectionString("default")!;
                services.AddSingleton(new UnicepsePrepAPIKey(""));
                services.AddHttpClient<UnicepseApiPrepHttpClient>(client =>
                {
                    string baseUrl = SyncSettingsManager.Current.ServerBaseUrl;

                    if (!string.IsNullOrWhiteSpace(baseUrl))
                    {
                        if (!baseUrl.EndsWith("/"))
                            baseUrl += "/";

                        client.BaseAddress = new Uri(baseUrl);
                    }

                    client.Timeout = TimeSpan.FromSeconds(SyncSettingsManager.Current.ConnectionTimeoutSeconds);
                });
                services.AddHttpClient<UnicepseApiClientV2>(c =>
                {
                    c.Timeout = TimeSpan.FromSeconds(30);
                    c.BaseAddress = new Uri(apiUrl!);
                });
                services.AddHostedService<DataSyncService>();
                services.AddSingleton<BackgroundServiceStore>();
                services.AddSingleton(new UnicepsDbContextFactory(CONNECTION_STRING, SQLITE));
                services.AddSingleton(new ActivationService(apiUrl, publicKey));
            });
            return _hostBuilder;
        }
    }
}
