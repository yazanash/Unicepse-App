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

namespace Unicepse.HostBuilders
{
    public static class AddViewModelHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                //services.AddTransient((s) => CreatePlayerListingViewModel(s)!);
                //services.AddSingleton<Func<PlayerListingViewModel>>((s) => () => s.GetRequiredService<PlayerListingViewModel>());
                //services.AddSingleton<NavigationService<PlayerListingViewModel>>();

                //services.AddTransient<MakePlayerViewModel>();
                //services.AddSingleton<Func<MakePlayerViewModel>>((s) => () => s.GetRequiredService<MakePlayerViewModel>());
                //services.AddSingleton<NavigationService<MakePlayerViewModel>>();

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<AuthViewModel>();
            });
            return _hostBuilder;
        }
        //private static PlayerListingViewModel? CreatePlayerListingViewModel(IServiceProvider s)
        //{
        //    return PlayerListingViewModel.LoadViewModel(
        //        s.GetRequiredService<NavigationService<MakePlayerViewModel>>(), s.GetRequiredService<GymStore>());
        //}

    }
}
