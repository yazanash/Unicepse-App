using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.BackgroundServices;
using Uniceps.Core.Common;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Services;
using Uniceps.MessengerSystem;
using Uniceps.MessengerSystem.Events;
using Uniceps.SystemServices;
using Windows.UI.Notifications;
using static Uniceps.Stores.PlayersDataStore;

namespace Uniceps.Stores
{
    public class PlayersAttendenceStore
    {
        public event Action<DailyPlayerReport>? LoggedIn;
        public event Action? Loaded;
        public event Action? PlayerLoggingLoaded;
        string LogFlag = "[Attendence] ";
        private readonly ILogger<PlayersAttendenceStore> _logger;
        public event Action<DailyPlayerReport>? LoggedOut;
        private readonly System.Media.SoundPlayer? _successSoundPlayer;
        public PlayersAttendenceStore(PlayersAttendenceService playersAttendenceService, ILogger<PlayersAttendenceStore> logger)
        {
            _playersAttendenceService = playersAttendenceService;
            _playersAttendence = new List<DailyPlayerReport>();
            _logger = logger;
            _successSoundPlayer = InitSuccessSound();
            Messenger.Default.Register<UserFingerprintScannedMessage>(this, LoginPlayerFromFingerPrint);
        }
        private System.Media.SoundPlayer? InitSuccessSound()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                // Get the stream for the embedded resource
                Stream? soundStream = assembly.GetManifestResourceStream("Uniceps.Resources.sounds.beep.wav");

                if (soundStream != null)
                {
                    var player = new System.Media.SoundPlayer(soundStream);
                    player.Load(); // 👈 تحميل الملف فوراً للذاكرة RAM
                    return player;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Attendence] فشل تحميل ملف الصوت: {ex.Message}");
            }

            return null;
        }
        private void PlaySuccessSound()
        {
            try
            {
                _successSoundPlayer?.Play();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Attendence] خطأ أثناء تشغيل الصوت: {ex.Message}");
            }
        }
        private async void LoginPlayerFromFingerPrint(UserFingerprintScannedMessage message)
        {
            DailyPlayerReport dailyPlayerReport = new DailyPlayerReport()
            {
                loginTime = DateTime.Now,
                logoutTime = DateTime.Now,
                Date = DateTime.Now,
                IsLogged = true,
                 PlayerId = message.PlayerId,
            };
          await  LogInPlayerWithId(dailyPlayerReport);
        }

        private readonly PlayersAttendenceService _playersAttendenceService;
        private readonly List<DailyPlayerReport> _playersAttendence;
        public IEnumerable<DailyPlayerReport> PlayersAttendence => _playersAttendence;


        private DailyPlayerReport? _selectedDailyPlayerReport;
        public DailyPlayerReport? SelectedDailyPlayerReport
        {
            get
            {
                return _selectedDailyPlayerReport;
            }
            set
            {
                _selectedDailyPlayerReport = value;
            }
        }

        public async Task LogInPlayer(DailyPlayerReport entity)
        {
            _logger.LogInformation(LogFlag + "login player");
            entity.DataStatus = DataStatus.ToCreate;
            await _playersAttendenceService.LogInPlayer(entity);

            _playersAttendence.Add(entity);
            LoggedIn?.Invoke(entity);
        }

        public async Task LogInPlayerWithId(DailyPlayerReport entity)
        {
           
           await Application.Current.Dispatcher.Invoke( async() =>
            {
                try
                {
                    _logger.LogInformation(LogFlag + "login player");
                    var existed = await _playersAttendenceService.GetByPlayerId(entity);
                    if (existed != null)
                    {
                        existed.IsLogged = false;
                        existed.logoutTime = DateTime.Now;
                        await LogOutPlayer(existed);
                    }
                    else
                    {
                        await _playersAttendenceService.LogInPlayerWithId(entity);
                        _playersAttendence.Add(entity);
                        LoggedIn?.Invoke(entity);
                    }
                     
                    PlaySuccessSound();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        public async Task LogOutPlayer(DailyPlayerReport entity)
        {
            _logger.LogInformation(LogFlag + "logout player");
            if (entity.DataStatus != DataStatus.ToCreate)
                entity.DataStatus = DataStatus.ToUpdate;
            bool loggedOut = await _playersAttendenceService.LogOutPlayer(entity);

            int currentIndex = _playersAttendence.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _playersAttendence[currentIndex] = entity;
            }
            else
            {
                _playersAttendence.Add(entity);
            }
            LoggedOut?.Invoke(entity);
        }
        public async Task AddKeyToPlayer(DailyPlayerReport entity)
        {
            _logger.LogInformation(LogFlag + "add key to player");
            DailyPlayerReport loggedOut = await _playersAttendenceService.AddKey(entity);

            int currentIndex = _playersAttendence.FindIndex(y => y.Id == entity.Id);

            if (currentIndex != -1)
            {
                _playersAttendence[currentIndex] = entity;
            }
            else
            {
                _playersAttendence.Add(entity);
            }
            LoggedOut?.Invoke(entity);
        }
        public async Task GetLoggedPlayers(DateTime date)
        {
            _logger.LogInformation(LogFlag + "get logs history");
            IEnumerable<DailyPlayerReport> LoggedPlayers = await _playersAttendenceService.GetLoggedPlayers(date);
            _playersAttendence.Clear();
            _playersAttendence.AddRange(LoggedPlayers);
            Loaded?.Invoke();
        }
        public async Task<DailyPlayerReport?> GetLoggedPlayer(DailyPlayerReport entity)
        {
            _logger.LogInformation(LogFlag + "get player log");
            return await _playersAttendenceService.Get(entity);
        }
        public async Task GetPlayerLogging(int playerid)
        {
            _logger.LogInformation(LogFlag + "get player logs");
            IEnumerable<DailyPlayerReport> subscriptions = await _playersAttendenceService.GetPlayerLogging(playerid);
            _playersAttendence.Clear();
            _playersAttendence.AddRange(subscriptions);
            PlayerLoggingLoaded?.Invoke();
        }
        public Task Initialize()
        {
            throw new NotImplementedException();

        }

        
    }
}
