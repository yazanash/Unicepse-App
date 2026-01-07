using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Uniceps.BackgroundServices;
using Uniceps.Core.Common;
using Uniceps.Entityframework.DbContexts;
using Uniceps.Helpers;
using Uniceps.HostBuilders;
using Uniceps.Stores;
using Uniceps.Stores.RoutineStores;
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
        private static Mutex? mutex = null;
        private static readonly string currentVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
        string LogFlag = "[App] ";
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private SplashScreenWindow? splashScreen;
        public App()
        {

            _host = CreateHostBuilder().Build();
            AuthViewModel auth = _host.Services.GetRequiredService<AuthViewModel>();
            auth.LoginAction += Auth_LoginAction;
        }

        private async Task CheckForUpdates()
        {
            try
            {
                var update = await Updater.CheckForUpdate();
                if (update!=null)
                {
                    ProgressWindow progressWindow = new ProgressWindow();
                    progressWindow.DataContext = new UpdateWindowViewModel(update);
                    progressWindow.Topmost = true;
                    progressWindow.ShowDialog();
                }
            }
            catch { }

        }
      
       
        private void Auth_LoginAction()
        {
            if (_host.Services.GetRequiredService<AccountStore>().CurrentAccount != null)
            {
                //MainWindowViewModel main = _host.Services.GetRequiredService<MainWindowViewModel>();
                _host.Services.GetRequiredService<AuthenticationStore>().LogoutAction += Auth_LogoutAction;
                //main.openLog();
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
            //await  _host.Services.GetRequiredService<AuthViewModel>().openLog();
            AuthWindow auth = _host.Services.GetRequiredService<AuthWindow>();
            AuthViewModel authmod = _host.Services.GetRequiredService<AuthViewModel>();
            authmod.LoginAction += Auth_LoginAction;
            auth.Show();
            authen.Close();
        }

        public void ResetHost()
        {
            _host.Dispose();
            _host.StopAsync();
            _host = CreateHostBuilder().Build();
            _host.Start();
        }
        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && !textBox.IsKeyboardFocusWithin)
            {
                textBox.Focus();
                e.Handled = true;
                textBox.SelectAll();
            }
        }

        public void CheckOpenedApplication()
        {
            const string appName = "Unicepse";
            bool createdNew;
            mutex = new Mutex(true, appName, out createdNew);
            if (!createdNew)
            {
                BringExistingInstanceToFront();
                Application.Current.Shutdown();
            }
        }
        public async Task PrepareDatabase()
        {
            var logger = _host.Services.GetRequiredService<ILogger<App>>();
            UnicepsDbContextFactory platinumGymDbContextFactory = _host.Services.GetRequiredService<UnicepsDbContextFactory>();
            using (UnicepsDbContext platinumGymDbContext = platinumGymDbContextFactory.CreateDbContext())
            {
                logger.LogInformation(LogFlag + "database migration started");
                platinumGymDbContext.Database.Migrate();
                #region dataConverter
                //platinumGymDbContext.Database.EnsureCreated();
                //logger.LogInformation(LogFlag + "get Files");
                //string fileName = "DBFILES/Player.csv";
                //string subsFileName = "DBFILES/Subscriptions.csv";
                //string sportFileName = "DBFILES/Sports.csv";
                //string trainerFileName = "DBFILES/trainers.csv";
                //string paymetsFileName = "DBFILES/payments.csv";
                //string trainersCreditsFileName = "DBFILES/trainersCredits.csv";
                //string jsonString = File.ReadAllText(fileName);
                //List<Exercises> exercises = JsonConvert.DeserializeObject<List<Exercises>>(jsonString)!;
                //    logger.LogInformation(LogFlag + "get players started");
                //    #region Player
                //    List<Player> players = File.ReadAllLines(fileName).Skip(1)
                //    .Select(line =>
                //   {

                //       var columns = line.Split(',');
                //       Player player = new Player();

                //       player.Id = Convert.ToInt32(columns[0]);

                //       player.FullName = columns[5].Replace("\"", "").Trim();

                //       player.Balance = Convert.ToDouble(columns[12]);

                //       player.BirthDate = Convert.ToDateTime(columns[4]).Year;

                //       player.GenderMale = Convert.ToBoolean(Convert.ToInt32(columns[10]));

                //       player.Hieght = Convert.ToDouble(columns[2]);

                //       player.Phone = columns[6].Replace("\"", "").Trim();

                //       player.IsSubscribed = Convert.ToBoolean(Convert.ToInt32(columns[9]));

                //       player.IsTakenContainer = Convert.ToBoolean(Convert.ToInt32(columns[8]));

                //       player.SubscribeDate = Convert.ToDateTime(columns[3]);

                //       player.SubscribeEndDate = Convert.ToDateTime(columns[7]);

                //       player.Weight = Convert.ToDouble(columns[1]);
                //       return player;

                //   }).ToList();
                //    #endregion
                //    logger.LogInformation(LogFlag + "get players Ended");
                //    logger.LogInformation(LogFlag + "get payments started");
                //    #region Payments
                //    List<PlayerPayment> payments = File.ReadAllLines(paymetsFileName).Skip(1)
                //   .Select(line =>
                //   {
                //       var columns = line.Split(',');
                //       PlayerPayment payment = new PlayerPayment();
                //       payment.PayDate = Convert.ToDateTime(columns[4]);
                //       payment.Player = new Player { Id = Convert.ToInt32(columns[2]) };
                //       payment.PaymentValue = Convert.ToDouble(columns[1]);
                //       payment.Des = columns[5];
                //       if (columns[6].Trim() != "NULL")
                //           payment.Subscription = new Subscription { Id = Convert.ToInt32(columns[6]) };
                //       return payment;

                //   }).ToList();
                //    #endregion
                //    logger.LogInformation(LogFlag + "get payments ended");
                //    logger.LogInformation(LogFlag + "get Credits started");
                //    #region Credits
                //    List<Credit> credits = File.ReadAllLines(trainersCreditsFileName).Skip(1)
                // .Select(line =>
                // {
                //     var columns = line.Split(',');
                //     Credit credits = new Credit();
                //     credits.CreditValue = Convert.ToDouble(columns[1]);
                //     credits.Description = "";
                //     credits.EmpPerson = new Employee() { Id = Convert.ToInt32(columns[3]) };
                //     credits.Date = Convert.ToDateTime(columns[2]);

                //     return credits;

                // }).ToList();
                //    #endregion
                //    logger.LogInformation(LogFlag + "get credits ended");
                //    logger.LogInformation(LogFlag + "get subscriptions started");
                //    #region Subscriptions
                //    List<Subscription> subscriptions = File.ReadAllLines(subsFileName).Skip(1)
                //    .Select(line =>
                // {
                //     var columns = line.Split(',');
                //     Subscription subscription = new Subscription();
                //     /////////// 0
                //     int sId = Convert.ToInt32(columns[0]);
                //     /////////// 4
                //     subscription.Sport = new Sport { Id = Convert.ToInt32(columns[4]) };
                //     /////////// 9
                //     subscription.LastCheck = Convert.ToDateTime(columns[9]);
                //     /////////// 5
                //     if (columns[5] != "NULL")
                //         subscription.Trainer = new Employee { Id = Convert.ToInt32(columns[5]) };
                //     //subscription.Trainer = Convert.ToInt32(columns[5]);
                //     /////////// 19
                //     subscription.PrevTrainer_Id = Convert.ToInt32(columns[19]);
                //     /////////// 3
                //     subscription.Player = new Player { Id = Convert.ToInt32(columns[3]) };
                //     /////////// 1
                //     subscription.RollDate = Convert.ToDateTime(columns[1]);
                //     /////////// 2
                //     subscription.Price = Convert.ToInt32(columns[2]);
                //     /////////// 22
                //     subscription.OfferValue = Convert.ToInt32(columns[22]);
                //     /////////// 21
                //     subscription.OfferDes = columns[21];
                //     /////////// 6
                //     subscription.PriceAfterOffer = Convert.ToInt32(columns[6]);
                //     /////////// 7
                //     subscription.MonthCount = Convert.ToInt32(columns[7]);
                //     /////////// 10
                //     subscription.IsPrivate = Convert.ToBoolean(Convert.ToInt32(columns[10]));
                //     /////////// 11
                //     subscription.IsPlayerPay = Convert.ToBoolean(Convert.ToInt32(columns[11]));
                //     /////////// 17
                //     subscription.IsStopped = Convert.ToBoolean(Convert.ToInt32(columns[17]));
                //     /////////// 18
                //     subscription.IsMoved = Convert.ToBoolean(Convert.ToInt32(columns[18]));
                //     /////////// 12
                //     subscription.PrivatePrice = Convert.ToInt32(columns[12]);
                //     /////////// 13
                //     subscription.IsPaid = Convert.ToBoolean(Convert.ToInt32(columns[13]));
                //     /////////// 14
                //     subscription.PaidValue = Convert.ToInt32(columns[14]);
                //     /////////// 20
                //     subscription.RestValue = Convert.ToInt32(columns[20]);
                //     /////////// 15
                //     subscription.EndDate = Convert.ToDateTime(columns[15]);
                //     /////////// 16
                //     subscription.LastPaid = Convert.ToDateTime(columns[16]);

                //     subscription.DaysCount = Convert.ToInt32(subscription.EndDate.Subtract(subscription.RollDate).TotalDays);

                //     logger.LogInformation(LogFlag + "get connect pays to subscriptions started");
                //     var pays = payments.Where(s => s.Subscription != null && s.Subscription!.Id == sId && s.PaymentValue > 0);
                //     foreach (var pay in pays)
                //     {
                //         subscription.Payments!.Add(pay);
                //     }

                //     return subscription;
                // }).ToList();
                //    logger.LogInformation(LogFlag + "get subscriptions ended");
                //    #endregion
                //    logger.LogInformation(LogFlag + "get sports started");
                //    #region Sports
                //    List<Sport> sports = File.ReadAllLines(sportFileName).Skip(1)
                //  .Select(line =>
                //  {
                //      var columns = line.Split(',');
                //      Sport sport = new Sport();
                //      sport.Id = Convert.ToInt32(columns[0]);
                //      sport.Name = columns[1].Replace("\"", "").Trim();
                //      sport.Price = Convert.ToDouble(columns[2]);
                //      sport.IsActive = true;
                //      sport.DaysInWeek = Convert.ToInt32(columns[4]);
                //      sport.DailyPrice = Convert.ToInt32(columns[5]);
                //      sport.DaysCount = Convert.ToInt32(columns[6]);

                //      return sport;

                //  }).ToList();
                //    #endregion
                //    logger.LogInformation(LogFlag + "get employees started");
                //    #region Trainers
                //    List<Employee> employees = File.ReadAllLines(trainerFileName).Skip(1)
                //.Select(line =>
                //{
                //    var columns = line.Split(',');

                //    Employee employee = new Employee();
                //    employee.Id = Convert.ToInt32(columns[0]);

                //    employee.Salary = Convert.ToBoolean(Convert.ToInt32(columns[1]));
                //    employee.Parcent = Convert.ToBoolean(Convert.ToInt32(columns[2]));
                //    employee.SalaryValue = Convert.ToDouble(columns[3]);
                //    employee.ParcentValue = Convert.ToInt32(columns[4]);
                //    employee.Position = "مدرب";
                //    employee.StartDate = Convert.ToDateTime(columns[12]);
                //    employee.Balance = 0;
                //    employee.IsActive = Convert.ToBoolean(Convert.ToInt32(columns[10]));
                //    employee.IsTrainer = true;
                //    employee.FullName = columns[6].Replace("\"", "").Trim();
                //    employee.Phone = columns[7].Replace("\"", "").Trim();
                //    employee.BirthDate = Convert.ToDateTime(columns[5]).Year;
                //    employee.GenderMale = Convert.ToBoolean(Convert.ToInt32(columns[9]));

                //    return employee;

                //}).ToList();
                //    #endregion
                //    logger.LogInformation(LogFlag + "add players started");
                //    // Add your seed data here
                //    //platinumGymDbContext.Players!.AddRange(players);
                //    foreach (var p in players)
                //    {
                //        var subs = subscriptions.Where(x => x.Player!.Id == p.Id);
                //        #region CopyPlayer
                //        Player player = new Player();
                //        player.FullName = p.FullName;

                //        player.Balance = p.Balance;

                //        player.BirthDate = p.BirthDate;

                //        player.GenderMale = p.GenderMale;

                //        player.Hieght = p.Hieght;

                //        player.Phone = p.Phone;

                //        player.IsSubscribed = p.IsSubscribed;

                //        player.IsTakenContainer = p.IsTakenContainer;

                //        player.SubscribeDate = p.SubscribeDate;

                //        player.SubscribeEndDate = p.SubscribeEndDate;

                //        player.Weight = p.Weight;
                //        #endregion
                //        platinumGymDbContext.Players!.Add(player);

                //        foreach (var s in subs)
                //        {
                //            s.Player = player;
                //        }
                //    }
                //    logger.LogInformation(LogFlag + "add players ended");
                //    logger.LogInformation(LogFlag + "add sports started");
                //    foreach (var p in sports)
                //    {
                //        var subs = subscriptions.Where(x => x.Sport!.Id == p.Id);
                //        #region CopySport
                //        Sport sport = new Sport();
                //        sport.Name = p.Name;
                //        sport.Price = p.Price;
                //        sport.IsActive = p.IsActive;
                //        sport.DaysInWeek = p.DaysInWeek;
                //        sport.DailyPrice = p.DailyPrice;
                //        sport.DaysCount = p.DaysCount;
                //        #endregion
                //        platinumGymDbContext.Sports!.Add(sport);
                //        foreach (var s in subs)
                //        {
                //            s.Sport = sport;
                //        }
                //    }
                //    logger.LogInformation(LogFlag + "add sports ended");
                //    logger.LogInformation(LogFlag + "add employee started");
                //    foreach (var p in employees)
                //    {
                //        var subs = subscriptions.Where(x => x.Trainer != null && x.Trainer.Id == p.Id);
                //        var cred = credits.Where(x => x.EmpPerson != null && x.EmpPerson.Id == p.Id);
                //        var prevsubs = subscriptions.Where(x => x.PrevTrainer_Id == p.Id);

                //        #region CopyTrainer
                //        Employee employee = new Employee();

                //        employee.Salary = p.Salary;
                //        employee.Parcent = p.Salary;
                //        employee.SalaryValue = p.SalaryValue;
                //        employee.ParcentValue = p.ParcentValue;
                //        employee.Position = p.Position;
                //        employee.StartDate = p.StartDate;
                //        employee.Balance = p.Balance;
                //        employee.IsActive = p.IsActive;
                //        employee.IsTrainer = p.IsTrainer;
                //        employee.FullName = p.FullName;
                //        employee.Phone = p.Phone;
                //        employee.BirthDate = p.BirthDate;
                //        employee.GenderMale = p.GenderMale;
                //        #endregion
                //        platinumGymDbContext.Employees!.Add(employee);
                //        foreach (var s in subs)
                //        {
                //            s.Trainer = employee;
                //        }
                //        foreach (var s in prevsubs)
                //        {
                //            s.PrevTrainer_Id = employee.Id;
                //        }
                //        foreach (var s in credits)
                //        {
                //            s.EmpPerson = employee;
                //        }
                //    }
                //    logger.LogInformation(LogFlag + "add employee ended");
                //    logger.LogInformation(LogFlag + "save started");
                //    platinumGymDbContext.SaveChanges();
                //    platinumGymDbContext.ChangeTracker.Clear();
                //    logger.LogInformation(LogFlag + "save ended");
                //    logger.LogInformation(LogFlag + "add credits started");
                //    foreach (var s in credits)
                //    {
                //        platinumGymDbContext.Attach(s.EmpPerson!);
                //    }
                //    logger.LogInformation(LogFlag + "add credit ended");
                //    platinumGymDbContext.Credit!.AddRange(credits);
                //    logger.LogInformation(LogFlag + "save started");
                //    platinumGymDbContext.SaveChanges();
                //    platinumGymDbContext.ChangeTracker.Clear();

                //    logger.LogInformation(LogFlag + "save ended");
                //    logger.LogInformation(LogFlag + "add subscription started");
                //    int counter = 0;
                //    foreach (var s in subscriptions)
                //    {
                //        counter++;
                //        platinumGymDbContext.Attach(s.Player!);
                //        platinumGymDbContext.Attach(s.Sport!);
                //        if (s.Trainer != null)
                //        {
                //            platinumGymDbContext.Entry(s.Trainer).State = EntityState.Detached;
                //            platinumGymDbContext.Attach(s.Trainer);
                //        }

                //        DateTime lastpaid = s.RollDate;
                //        foreach (var p in s.Payments!.OrderBy(x => x.PayDate))
                //        {
                //            p.Player = s.Player;
                //            p.Subscription = s;
                //            p.From = lastpaid;
                //            int sportDays = s.DaysCount;
                //            double dayPrice = s.PriceAfterOffer / sportDays;
                //            int daysCount = Convert.ToInt32(p.PaymentValue / dayPrice);
                //            lastpaid = lastpaid.AddDays(daysCount);
                //            p.CoverDays = daysCount;
                //            p.To = lastpaid;
                //        }
                //        platinumGymDbContext.Subscriptions!.Add(s);
                //    }

                //    logger.LogInformation(LogFlag + "add subscription ended");
                //    logger.LogInformation(LogFlag + "save started");
                //    platinumGymDbContext.SaveChanges();
                //    logger.LogInformation(LogFlag + "save ended");
                //IApiDataStore<Subscription> _subsApiDataStore = _host.Services.GetRequiredService<IApiDataStore<Subscription>>();
                //IApiDataStore<Player> _apiDataStore = _host.Services.GetRequiredService<IApiDataStore<Player>>();
                //IApiDataStore<PlayerPayment> _paymentsApiDataStore = _host.Services.GetRequiredService<IApiDataStore<PlayerPayment>>();
                //IEnumerable<Player> playersList =  platinumGymDbContext.Players!.ToList();
                //logger.LogInformation(LogFlag + "add players to sync started");
                //foreach (var p in playersList) { await _apiDataStore.Create(p); }
                //logger.LogInformation(LogFlag + "add players to sync ended");
                //logger.LogInformation(LogFlag + "add subscription to sync started");
                //IEnumerable<Subscription> subscriptionList = platinumGymDbContext.Subscriptions!.Include(x=>x.Player).Include(x => x.Sport)
                //    .Include(x => x.Trainer).ToList();
                //foreach (var s in subscriptionList) { await _subsApiDataStore.Create(s); }
                //logger.LogInformation(LogFlag + "add subscription to sync ended");
                //logger.LogInformation(LogFlag + "add pays to sync started");
                //IEnumerable<PlayerPayment> PaymentsList = platinumGymDbContext.PlayerPayments!.Include(x => x.Player).Include(x => x.Subscription).ToList();
                //foreach (var pay in PaymentsList) { await _paymentsApiDataStore.Create(pay); }
                //logger.LogInformation(LogFlag + "add pays to sync ended");
                #endregion
                SplashScreenViewModel splash = _host.Services.GetRequiredService<SplashScreenViewModel>();
                logger.LogInformation(LogFlag + "database migrate successfully");
                bool internetAvailable = InternetAvailability.IsInternetAvailable();
                if (internetAvailable)
                {
                    try
                    {

                        ExercisesDataStore exercisesDataStore = _host.Services.GetRequiredService<ExercisesDataStore>();
                        exercisesDataStore.MuscleGroupDownloaded += ExercisesDataStore_MuscleGroupDownloaded; ;
                        exercisesDataStore.GotExercises += ExercisesDataStore_GotExercises;
                        await exercisesDataStore.GetExcersisesWithMuscleGroups();
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void ExercisesDataStore_MuscleGroupDownloaded(double obj)
        {
            SplashScreenViewModel splash = _host.Services.GetRequiredService<SplashScreenViewModel>();
            ExCount = obj;
            splash.Progress = 0;
        }

        public double ExCount = 0;
        private void ExercisesDataStore_GotExercises(double obj)
        {
            SplashScreenViewModel splash = _host.Services.GetRequiredService<SplashScreenViewModel>();
            splash.Message = "جاري تحميل التمارين";
            splash.Progress = (obj / ExCount) * 100;
        }


        public async Task OpenMainView()
        {
            AuthenticationStore authenticationStore = _host.Services.GetRequiredService<AuthenticationStore>();
            if (await authenticationStore.HasUser())
            {
                await _host.Services.GetRequiredService<AuthViewModel>().openLog();
                AuthWindow auth = _host.Services.GetRequiredService<AuthWindow>();
                auth.Show();

            }
            else
            {
                MainWindow mainWindow = _host.Services.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }

        }
        public void OpenLoginView()
        {
            LicenseWindow auth = _host.Services.GetRequiredService<LicenseWindow>();
            auth.Show();
        }

        public async Task PerpareApp()
        {
            //try
            //{
                SplashScreenViewModel splash = _host.Services.GetRequiredService<SplashScreenViewModel>();
               await CheckForUpdates();
                CheckOpenedApplication();
              
                _host.Start();
                var logger = _host.Services.GetRequiredService<ILogger<App>>();
                logger.LogInformation(LogFlag + "services started");
                await PrepareDatabase();
                logger.LogInformation(LogFlag + "get licenses");
                SessionValidator sysStore = _host.Services.GetRequiredService<SessionValidator>();
                UserFlowService userFlowService = _host.Services.GetRequiredService<UserFlowService>();
                bool hasValidSession = await sysStore.HasValidSession();

                AccountStore accountStore = _host.Services.GetRequiredService<AccountStore>();
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
                splash.Message = " التطبيق جاهز للعمل ...";
                logger.LogInformation(LogFlag + "Token is valid. Opening main view...");
                await userFlowService.RefreshUserContextAsync();
                await OpenMainView();
            //}
            //catch (Exception ex)
            //{
            //    var logger = _host.Services.GetRequiredService<ILogger<App>>();
            //    logger.LogInformation(LogFlag + "exeption throw on app startup {0}", ex.Message);
            //    MessageBox.Show(ex.Message);
            //}
        }
        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var savedTheme = Uniceps.Properties.Settings.Default.AppTheme;
            if (Enum.TryParse(savedTheme, out AppTheme theme))
            {
                ThemeService.ApplyTheme(theme);
            }

            // ثم تابع تحميل الواجهة
            splashScreen = new SplashScreenWindow();
            splashScreen.DataContext = _host.Services.GetRequiredService<SplashScreenViewModel>();
            splashScreen.Show();
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(TextBox_PreviewMouseLeftButtonDown));
            await PerpareApp();
            splashScreen.Close();
        }
        public static IHostBuilder CreateHostBuilder() =>
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
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<DataSyncService>();
                    string? CONNECTION_STRING = hostContext.Configuration.GetConnectionString("default");
                    bool SQLITE = hostContext.Configuration.GetValue<bool>("UseSqlite");
                    if (SQLITE)
                        CONNECTION_STRING = hostContext.Configuration.GetConnectionString("sqlite");
                    services.AddSingleton(new UnicepsDbContextFactory(CONNECTION_STRING, SQLITE));
                });

        private void BringExistingInstanceToFront()
        {
            var currentProcess = Process.GetCurrentProcess();
            foreach (var process in Process.GetProcessesByName(currentProcess.ProcessName))
            {
                if (process.Id != currentProcess.Id)
                {
                    SetForegroundWindow(process.MainWindowHandle);
                    break;
                }
            }
        }
        protected override void OnExit(ExitEventArgs e)
        {
            _host.Services.GetRequiredService<AuthenticationStore>().Logout();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
