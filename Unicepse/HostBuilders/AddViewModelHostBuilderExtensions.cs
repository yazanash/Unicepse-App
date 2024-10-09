using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Unicepse.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;
using Unicepse.ViewModels.Authentication;
using Unicepse.ViewModels._ِAppViewModels;
using Unicepse.Views.AuthView;

namespace Unicepse.HostBuilders
{
    public static class AddViewModelHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<AuthViewModel>();
                services.AddSingleton<LicenseViewModel>();
                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>(),
                });
                services.AddSingleton(s => new AuthWindow()
                {
                    DataContext = s.GetRequiredService<AuthViewModel>(),
                });
                services.AddSingleton(s => new LicenseWindow()
                {
                    DataContext = s.GetRequiredService<LicenseViewModel>(),
                });
            });
            return _hostBuilder;
        }
      
    }
}
