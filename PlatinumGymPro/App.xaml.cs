using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlatinumGym.Core.Models.Authentication;
using PlatinumGym.Core.Services;
using PlatinumGym.Entityframework.DbContexts;
using PlatinumGym.Entityframework.Services;
using PlatinumGym.Entityframework.Services.AuthService;
using PlatinumGym.Entityframework.Services.PlayerQueries;
using PlatinumGymPro.HostBuilders;
//using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Services.PlayerConflictValidators;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels;
using PlatinumGymPro.Views.AuthView;
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
                   services.AddSingleton(new NavigationStore());
                   services.AddSingleton<IPasswordHasher, PasswordHasher>();
                   services.AddSingleton<IAccountDataService<User>, AccountDataService>();
                   services.AddSingleton<AuthenticationService>();
                   services.AddSingleton<AccountStore>();
                   services.AddSingleton<PlayersDataStore>();
                   services.AddSingleton<PlayerDataService>();
                   services.AddSingleton<SportDataStore>();
                   services.AddSingleton<SportServices>();
                   services.AddSingleton<EmployeeDataService>();
                   services.AddSingleton<EmployeeStore>();
                   services.AddSingleton<AuthenticationStore>();
                   services.AddSingleton(s => new MainWindow()
                   {
                       DataContext = s.GetRequiredService<MainWindowViewModel>(),
                   });
                   services.AddSingleton(s => new AuthWindow()
                   {
                       DataContext = s.GetRequiredService<AuthViewModel>(),
                   });
               }).Build();
            AuthViewModel auth = _host.Services.GetRequiredService<AuthViewModel>();
            auth.LoginAction += Auth_LoginAction;





        }
      
        private void Auth_LoginAction()
        {
            if (_host.Services.GetRequiredService<AccountStore>().CurrentAccount != null)
            {
                MainWindow auth = _host.Services.GetRequiredService<MainWindow>();
                Application.Current.MainWindow.Close();

                auth.Show();
            }
            else
            {
                AuthWindow auth = _host.Services.GetRequiredService<AuthWindow>();
                auth.Show();

            }

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            PlatinumGymDbContextFactory platinumGymDbContextFactory = _host.Services.GetRequiredService<PlatinumGymDbContextFactory>();
            using (PlatinumGymDbContext platinumGymDbContext = platinumGymDbContextFactory.CreateDbContext())
            {
                platinumGymDbContext.Database.Migrate();
            }

            MainWindow auth = _host.Services.GetRequiredService<MainWindow>();
            auth.Show();


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
