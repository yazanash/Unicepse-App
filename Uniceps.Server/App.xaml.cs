using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;
using Uniceps.Server.Extensions;
using Uniceps.Server.SystemServices;

namespace Uniceps.Server
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost? _host;
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            FirewallHelper.OpenPortInFirewall(5800);
            await Task.Run(async () =>
            {
                var builder = WebApplication.CreateBuilder(e.Args);

                builder.Services.AddControllers();
                builder.Services.AddUniceps(builder.Configuration);

                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(5800);
                });

                var app = builder.Build();

                app.MapControllers();
                using (var scope = app.Services.CreateScope())
                {
                    var initializer = scope.ServiceProvider
                        .GetRequiredService<DatabaseInitialService>();

                    await initializer.InitializeAsync();
                }

                _host = app;

                app.Run();
            });
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
            }
            base.OnExit(e);
        }
    }

}
