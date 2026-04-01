using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uniceps.BackgroundServices;
using Uniceps.Entityframework.DbContexts;
using Uniceps.HostBuilders;
using Uniceps.LicenseManager;

namespace Uniceps.SystemServices
{
    public static class HostConfigurator
    {
        private static readonly string currentVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();

        public static IHost Build() =>
           Host.CreateDefaultBuilder()
              .UseSerilog((host, loggerConfiguration) =>
              {
                  loggerConfiguration.WriteTo.File("logs/logs-.txt", rollingInterval: RollingInterval.Day,
                       outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] ({Version}) {Message}{NewLine}{Exception}")
                  .Enrich.WithProperty("Version", currentVersion)
                  .WriteTo.Debug(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] ({Version}) {Message}{NewLine}{Exception}")
                  .MinimumLevel.Error()
                  .MinimumLevel.Override("Unicepse", Serilog.Events.LogEventLevel.Debug);
              })
              .AddViewModels()
              .AddServices()
              .AddApiServices()
              .AddDataStatusServices()
              .AddDataStatusStores()
              .AddCommands()
              .AddStores()
              .AddHttpClient()
              .Build();

    }
}
