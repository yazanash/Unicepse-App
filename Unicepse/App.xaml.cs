﻿using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Newtonsoft.Json;
using Unicepse.Core.Models.Authentication;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Sport;
using Unicepse.Core.Models.Subscription;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;
using Unicepse.Entityframework.Services;
using Unicepse.Entityframework.Services.AuthService;
using Unicepse.Entityframework.Services.PlayerQueries;
using Unicepse.Views.AuthView;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.navigation.Stores;
using Unicepse.Stores;
using Unicepse.HostBuilders;
using Unicepse.ViewModels.Authentication;
using Unicepse.utlis.common;
using Serilog;
using Microsoft.Extensions.Logging;
using Unicepse.BackgroundServices;
using Unicepse.API.Models;
using Unicepse.API;
using Unicepse.API.Services;
using Unicepse.ViewModels._ِAppViewModels;

namespace Unicepse
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
                   services.AddHostedService<DataSyncService>();
                   string? CONNECTION_STRING = hostContext.Configuration.GetConnectionString("default"); ;
                   services.AddSingleton(new PlatinumGymDbContextFactory(CONNECTION_STRING));
                   services.AddSingleton(new NavigationStore());
                   services.AddSingleton<IPasswordHasher, PasswordHasher>();
                   services.AddSingleton<IAccountDataService<User>, AccountDataService>();
                   services.AddSingleton<AuthenticationService>();
                   services.AddSingleton<AccountStore>();
                   services.AddSingleton<UsersDataStore>();
                   services.AddSingleton<PlayersDataStore>();
                   services.AddSingleton<PlayerDataService>();
                   services.AddSingleton<SportDataStore>();
                   services.AddSingleton<SportServices>();
                   services.AddSingleton<EmployeeDataService>();
                   services.AddSingleton<EmployeeStore>();
                   services.AddSingleton<BackgroundServiceStore>();

                   services.AddSingleton<GymStore>();

                   services.AddSingleton<DausesDataService>();
                   services.AddSingleton<EmployeeCreditsDataService>();
                   services.AddSingleton<CreditsDataStore>();
                   services.AddSingleton<DausesDataStore>();

                   services.AddSingleton<PaymentDataService>();
                   services.AddSingleton<PaymentDataStore>();

                   services.AddSingleton<PlayersAttendenceService>();
                   services.AddSingleton<PlayersAttendenceStore>();

                   services.AddSingleton<SubscriptionDataStore>();
                   services.AddSingleton<SubscriptionDataService>();

                   services.AddSingleton<ExpensesDataService>();
                   services.AddSingleton<ExpensesDataStore>();
                   services.AddSingleton<MetricDataService>();
                   services.AddSingleton<MetricDataStore>();

                   services.AddSingleton<PlayerRoutineDataService>();
                   services.AddSingleton<RoutineDataStore>();

                   services.AddSingleton<PlayerApiDataService>();
                   services.AddSingleton<PaymentApiDataService>();
                   services.AddSingleton<MetricApiDataService>();
                   services.AddSingleton<SubscriptionApiDataService>();

                   services.AddSingleton<LicenseApiDataService>();
                   services.AddSingleton<LicenseDataService>();
                   services.AddSingleton<LicenseDataStore>();


                   services.AddSingleton<AuthenticationStore>();
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
                   //string? apiKey = hostContext.Configuration.GetValue<string>("PUBLIC_API_KEY");
                   services.AddSingleton(new UnicepsePrepAPIKey(""));

                   services.AddHttpClient<UnicepseApiPrepHttpClient>(c =>
                   {
                       c.BaseAddress = new Uri("https://uniapi-ui65lw0m.b4a.run/api/v1/");
                       //c.DefaultRequestHeaders.Add("CustomHeaderName", "CustomHeaderValue");
                   });

               }).Build();
            AuthViewModel auth = _host.Services.GetRequiredService<AuthViewModel>();
            auth.LoginAction += Auth_LoginAction;

            LicenseViewModel license = _host.Services.GetRequiredService<LicenseViewModel>();
            license.LicenseAction += License_VerifiedAction;




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

        private void License_VerifiedAction()
        {
            if (_host.Services.GetRequiredService<LicenseDataStore>().CurrentLicense != null)
            {
                AuthWindow auth = _host.Services.GetRequiredService<AuthWindow>();
                Application.Current.MainWindow.Close();

                auth.Show();
            }
            else
            {
                LicenseWindow auth = _host.Services.GetRequiredService<LicenseWindow>();
                auth.Show();

            }

        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            try
            {

                _host.Start();
                PlatinumGymDbContextFactory platinumGymDbContextFactory = _host.Services.GetRequiredService<PlatinumGymDbContextFactory>();
                using (PlatinumGymDbContext platinumGymDbContext = platinumGymDbContextFactory.CreateDbContext())
                {
                    platinumGymDbContext.Database.Migrate();
                    #region dataConverter
                    //    platinumGymDbContext.Database.EnsureCreated();
                    //    string fileName = "DBFILES/Players.csv";
                    //    string subsFileName = "DBFILES/Subscriptions.csv";
                    //    string sportFileName = "DBFILES/Sports.csv";
                    //    string trainerFileName = "DBFILES/trainers.csv";
                    //    string paymetsFileName = "DBFILES/payments.csv";
                    //    string trainersCreditsFileName = "DBFILES/trainersCredits.csv";
                    //    //string jsonString = File.ReadAllText(fileName);
                    //    //List<Exercises> exercises = JsonConvert.DeserializeObject<List<Exercises>>(jsonString)!;

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

                    //    List<Subscription> subscriptions = File.ReadAllLines(subsFileName).Skip(1)
                    //    .Select(line =>
                    // {
                    //     var columns = line.Split(',');
                    //     Subscription subscription = new Subscription();
                    //         /////////// 0
                    //         int sId = Convert.ToInt32(columns[0]);
                    //         /////////// 4
                    //         subscription.Sport = new Sport { Id = Convert.ToInt32(columns[4]) };
                    //         /////////// 9
                    //         subscription.LastCheck = Convert.ToDateTime(columns[9]);
                    //         /////////// 5
                    //         if (columns[5] != "NULL")
                    //         subscription.Trainer = new Employee { Id = Convert.ToInt32(columns[5]) };
                    //         //subscription.Trainer = Convert.ToInt32(columns[5]);
                    //         /////////// 19
                    //         subscription.PrevTrainer_Id = Convert.ToInt32(columns[19]);
                    //         /////////// 3
                    //         subscription.Player = new Player { Id = Convert.ToInt32(columns[3]) };
                    //         /////////// 1
                    //         subscription.RollDate = Convert.ToDateTime(columns[1]);
                    //         /////////// 2
                    //         subscription.Price = Convert.ToInt32(columns[2]);
                    //         /////////// 22
                    //         subscription.OfferValue = Convert.ToInt32(columns[22]);
                    //         /////////// 21
                    //         subscription.OfferDes = columns[21];
                    //         /////////// 6
                    //         subscription.PriceAfterOffer = Convert.ToInt32(columns[6]);
                    //         /////////// 7
                    //         subscription.MonthCount = Convert.ToInt32(columns[7]);
                    //         /////////// 10
                    //         subscription.IsPrivate = Convert.ToBoolean(Convert.ToInt32(columns[10]));
                    //         /////////// 11
                    //         subscription.IsPlayerPay = Convert.ToBoolean(Convert.ToInt32(columns[11]));
                    //         /////////// 17
                    //         subscription.IsStopped = Convert.ToBoolean(Convert.ToInt32(columns[17]));
                    //         /////////// 18
                    //         subscription.IsMoved = Convert.ToBoolean(Convert.ToInt32(columns[18]));
                    //         /////////// 12
                    //         subscription.PrivatePrice = Convert.ToInt32(columns[12]);
                    //         /////////// 13
                    //         subscription.IsPaid = Convert.ToBoolean(Convert.ToInt32(columns[13]));
                    //         /////////// 14
                    //         subscription.PaidValue = Convert.ToInt32(columns[14]);
                    //         /////////// 20
                    //         subscription.RestValue = Convert.ToInt32(columns[20]);
                    //         /////////// 15
                    //         subscription.EndDate = Convert.ToDateTime(columns[15]);
                    //         /////////// 16
                    //         subscription.LastPaid = Convert.ToDateTime(columns[16]);

                    //     subscription.DaysCount = Convert.ToInt32(subscription.EndDate.Subtract(subscription.RollDate).TotalDays);

                    //     var pays = payments.Where(s => s.Subscription != null && s.Subscription!.Id == sId && s.PaymentValue > 0);
                    //     foreach (var pay in pays)
                    //     {
                    //         subscription.Payments!.Add(pay);
                    //     }

                    //     return subscription;
                    // }).ToList();


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
                    //    platinumGymDbContext.SaveChanges();
                    //    platinumGymDbContext.ChangeTracker.Clear();

                    //    foreach (var s in credits)
                    //    {
                    //        platinumGymDbContext.Attach(s.EmpPerson!);
                    //    }
                    //    platinumGymDbContext.Credit!.AddRange(credits);
                    //    platinumGymDbContext.SaveChanges();
                    //    platinumGymDbContext.ChangeTracker.Clear();
                    //    // Add more instances as needed
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
                    //        if (counter % 100 == 0)
                    //        {

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
                    #endregion

                    if (!platinumGymDbContext.Exercises!.Any())
                    {
                        string f_name = "Training.json";
                        using (StreamReader r = new StreamReader(f_name))
                        {
                            string json = r.ReadToEnd();
                            List<Exercises>? data = JsonConvert.DeserializeObject<List<Exercises>>(json);
                            foreach (Exercises ex in data!)
                                ex.Id = 0;
                            await platinumGymDbContext.Exercises!.AddRangeAsync(data!);

                        }

                        platinumGymDbContext.SaveChanges();
                    }
                }
                LicenseDataStore licenseDataStore = _host.Services.GetRequiredService<LicenseDataStore>();
                licenseDataStore.ActiveLicense();
                if (licenseDataStore.CurrentLicense != null)
                {
                    _host.Services.GetRequiredService<UnicepsePrepAPIKey>().updateToken(licenseDataStore.CurrentLicense.Token!);
                    bool internetAvailable = InternetAvailability.IsInternetAvailable();
                    if (internetAvailable)
                    {
                        await licenseDataStore.CheckLicenseValidation();
                        if (licenseDataStore.CurrentLicense != null)
                        {
                            _host.Services.GetRequiredService<AuthViewModel>().openLog();
                            AuthWindow auth = _host.Services.GetRequiredService<AuthWindow>();
                            auth.Show();
                        }
                        else
                        {
                            LicenseWindow auth = _host.Services.GetRequiredService<LicenseWindow>();
                            auth.Show();
                        }
                    }
                    else
                    {
                        _host.Services.GetRequiredService<AuthViewModel>().openLog();
                        AuthWindow auth = _host.Services.GetRequiredService<AuthWindow>();
                        auth.Show();
                    }

                }
                else
                {
                    LicenseWindow auth = _host.Services.GetRequiredService<LicenseWindow>();
                    auth.Show();
                }


                //CameraReader cameraReader = new CameraReader();
                //cameraReader.Show();
                //MessageBox.Show(System.Environment.CurrentDirectory) ;
                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
