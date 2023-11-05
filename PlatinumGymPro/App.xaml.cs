using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlatinumGym.Entityframework.DbContexts;
using PlatinumGymPro.HostBuilders;
//using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Services.PlayerConflictValidators;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        //private SelectedPlayerStore _selectedPlayerStore;
        //private ModalNavigationStore _modalNavigationStore;
        public App()
        {
            //_selectedPlayerStore = new();
            //_modalNavigationStore = new();
            _host = Host.CreateDefaultBuilder()
                .AddViewModels()
                .ConfigureServices((hostContext, services) =>
               {

                   string? CONNECTION_STRING = hostContext.Configuration.GetConnectionString("default"); ;
                   services.AddSingleton(new PlatinumGymDbContextFactory(CONNECTION_STRING));
                   //services.AddSingleton<IPlayerProvider, DatabasePlayerProvider>();
                   //services.AddSingleton<IPlayerCreator, DatabasePlayerCreator>();

                   //services.AddSingleton<IPlayerConflictValidator, DatabasePlayerConflictValidator>();
                   ////services.AddTransient<PlayersBook>();
                   //services.AddSingleton<PlayerDataService>();
                   //services.AddSingleton<Stores.PlayerStores.PlayerStore>();
                   //services.AddSingleton<SportServices>();
                   //services.AddSingleton<Stores.SportStore>();

                   //services.AddSingleton<GenericDataService<Employee>>();
                   //services.AddSingleton<Stores.TrainerStore>();
                   //services.AddSingleton<Stores.NavigationStore>();
                   //services.AddSingleton((s) => new Gym(s.GetRequiredService<PlayersBook>()));
                   //services.AddSingleton<GymStore>();
                   //services.AddSingleton<ModalNavigationStore>();


                   //services.AddSingleton<NavigationStore>();

                   services.AddSingleton(s => new MainWindow()
                   {
                       DataContext = s.GetRequiredService<MainWindowViewModel>(),
                   });
               }).Build();






        }


        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            PlatinumGymDbContextFactory platinumGymDbContextFactory = _host.Services.GetRequiredService<PlatinumGymDbContextFactory>();
            using (PlatinumGymDbContext platinumGymDbContext = platinumGymDbContextFactory.CreateDbContext())
            {
                platinumGymDbContext.Database.Migrate();
            }

            //NavigationService<PlayerListingViewModel> navigationService = _host.Services.GetRequiredService<NavigationService<PlayerListingViewModel>>();
            //navigationService.Navigate();
            MainWindow main = _host.Services.GetRequiredService<MainWindow>();
            main.Show();

            base.OnStartup(e);
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<PlatinumGymDbContextFactory>();

            return services.BuildServiceProvider();
        }


        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();
            base.OnExit(e);
        }

    }
}
