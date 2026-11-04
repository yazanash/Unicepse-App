using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.BackgroundServices;
using Uniceps.BioEngineServices;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Services.PlayerQueries;
using Uniceps.Helpers;
using Uniceps.LicenseManager;
using Uniceps.Stores;
using Uniceps.SystemServices;
using Uniceps.ViewModels;
using Uniceps.ViewModels.Authentication;
using Uniceps.Views;
using Uniceps.Views.AuthView;

namespace Uniceps
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {
        private IHost _host;
        public App()
        {

            _host = HostConfigurator.Build();
            LoginViewModel auth = _host.Services.GetRequiredService<LoginViewModel>();
            auth.LoginAction += Auth_LoginAction;
            SetupGlobalExceptions();
        }
      
        private void EnsureGuestAccount()
        {
            var accountStore = _host.Services.GetRequiredService<AccountStore>();
            if (accountStore.CurrentAccount == null)
            {
                accountStore.CurrentAccount = new Core.Models.Authentication.User()
                {
                    OwnerName = "مستخدم غير مسجل",
                    Role = Roles.Admin,
                    UserName = "زائر",
                    Position = "زائر",
                };
            }
        }

        private void SetupGlobalExceptions()
        {
            this.DispatcherUnhandledException += (s, e) =>
            {
                LogAndShowException(e.Exception, "UI Thread Exception");
                e.Handled = true;
                Application.Current.Shutdown();

            };
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                LogAndShowException((Exception)e.ExceptionObject, "Non-UI Exception");
                Application.Current.Shutdown();

            };
        }
       
        private void LogAndShowException(Exception ex, string type)
        {
            _host.Services.GetRequiredService<ILogger<App>>().LogCritical(ex, type);
            MessageBox.Show($"حدث خطأ غير متوقع: {ex.Message}", "خطأ في النظام");
        }

        private void Auth_LoginAction()
        {
            if (_host.Services.GetRequiredService<AccountStore>().CurrentAccount != null)
            {
                _host.Services.GetRequiredService<AuthenticationStore>().LogoutAction += Auth_LogoutAction;
                MainWindow auth = _host.Services.GetRequiredService<MainWindow>();

                AuthWindow authentication = _host.Services.GetRequiredService<AuthWindow>();
                authentication.Close();
                auth.Show();
            }
            else
            {
                AuthWindow auth = _host.Services.GetRequiredService<AuthWindow>();
                auth.Show();
            }

        }
        private void Auth_LogoutAction()
        {
            MainWindow authen = _host.Services.GetRequiredService<MainWindow>();
            ResetHost();
            AuthWindow auth = _host.Services.GetRequiredService<AuthWindow>();
            LoginViewModel authmod = _host.Services.GetRequiredService<LoginViewModel>();
            authmod.LoginAction += Auth_LoginAction;
            auth.Show();
            authen.Close();
        }

        public void ResetHost()
        {
            _host.Dispose();
            _host.StopAsync();
            _host = HostConfigurator.Build();
            _host.Start();
        }

        public async Task OpenMainView()
        {
            AuthenticationStore authenticationStore = _host.Services.GetRequiredService<AuthenticationStore>();
            if (await authenticationStore.HasUser())
            {
                AuthWindow auth = _host.Services.GetRequiredService<AuthWindow>();
                auth.Show();

            }
            else
            {
                MainWindow mainWindow = _host.Services.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }

        }
        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SystemManager.InitializeSystem();
            CustomLiveChartsExtensions.AddLiveChartsAppSettings();
            var activationService = _host.Services.GetRequiredService<ActivationService>();
            LicenseActivationService licenseActivationService = _host.Services.GetRequiredService<LicenseActivationService>();
            if (e.Args.Length > 0 && Path.GetExtension(e.Args[0]).ToLower() == ".unxlic")
            {
                await licenseActivationService.HandleFileActivation(e.Args[0]);
                return;
            }
           
            LicenseStore licenseStore = _host.Services.GetRequiredService<LicenseStore>();
            licenseStore.Update(activationService.GetCurrentLicenseStatus());
            SplashScreenWindow splashScreen = new SplashScreenWindow();
            splashScreen.DataContext = _host.Services.GetRequiredService<SplashScreenViewModel>();
            splashScreen.Show();


            SplashScreenViewModel splash = _host.Services.GetRequiredService<SplashScreenViewModel>();
            await Updater.RunUpdater();
            var logger = _host.Services.GetRequiredService<ILogger<App>>();
            DatabaseInitialService databaseInitialService = _host.Services.GetRequiredService<DatabaseInitialService>();
            await databaseInitialService.SetupAsync();
            try
            {
                var bioEngine = _host.Services.GetRequiredService<BioEngine>();
                bioEngine.StartDevice();
                var playerService = _host.Services.GetRequiredService<IDataService<Player>>();
                var players = await playerService.GetAll();
                bioEngine.LoadUsersToRam(players.ToList());
            }
            catch { }
            ExerciseSyncService exerciseSyncService = _host.Services.GetRequiredService<ExerciseSyncService>();
            await exerciseSyncService.SyncIfConnectedAsync();
            _host.Start();
         
            EnsureGuestAccount();
            splash.Message = " التطبيق جاهز للعمل ...";
            await OpenMainView();
            splashScreen.Close();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                var bioEngine = _host.Services.GetService<BioEngine>();
                bioEngine?.StopDevice();
            }
            catch
            {
                // Ignore on shutdown
            }

            _host.Services.GetRequiredService<AuthenticationStore>().Logout();
            _host.Dispose();
           
            base.OnExit(e);
        }
    }
}
