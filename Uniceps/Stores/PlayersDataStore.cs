using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Uniceps.BackgroundServices;
using Uniceps.Core.Common;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Services.PlayerQueries;
using Uniceps.MessengerSystem;
using Uniceps.MessengerSystem.Events;
using Uniceps.Services;
using Uniceps.Stores.ApiDataStores;
using Uniceps.utlis.common;
using Uniceps.ViewModels.PlayersViewModels;

namespace Uniceps.Stores
{

    public class PlayersDataStore
    {
        private readonly IDataService<Player> _playerDataService;
        private readonly LicenseStore _licenseStore;
        private readonly IExcelService<Player> _excelService;
        private readonly List<Player> _players;
        private readonly Lazy<Task> _initializeLazy;
        private readonly ILogger<PlayersDataStore> _logger;
        public IEnumerable<Player> Players => _players;
        public event Action<Player>? Player_created;
        public event Action? Players_loaded;
        public event Action<Player>? Player_update;
        public event Action<int>? Player_deleted;
        public PlayersDataStore(IDataService<Player> playerDataService, ILogger<PlayersDataStore> logger,IExcelService<Player> excelService, LicenseStore licenseStore)
        {
            _playerDataService = playerDataService;
            _players = new List<Player>();
            _initializeLazy = new Lazy<Task>(Initialize);
            _logger = logger;
            _excelService = excelService;
            _licenseStore = licenseStore;
        }

        public List<Player> ImportFromExcel(string filePath)
        {
            //ImportStarted?.Invoke();
            List<Player> players = _excelService.ImportFromExcel(filePath);
            //DataImported?.Invoke(players.Count());
            return players;
        }
        public void ExportToExcel(string filePath)
        {
            //ExportStarted?.Invoke();
            _excelService.ExportToExcel(filePath, _players);
            //DataExported?.Invoke(_players.Count);
        }
        public async Task GetAll()
        {
            _logger.LogInformation("get players");
            await _initializeLazy.Value;
            Players_loaded?.Invoke();
        }
       
        public async Task AddPlayer(Player player)
        {
            if (!_licenseStore.Current.IsFullVersion && _players.Count() >= 50)
                throw new FreeLimitException("لقد وصلت الحد الاعلى من النسخة المجانية ... اشترك الان لتحصل عدد غير محدود");

                _logger.LogInformation("add player");
            player.DataStatus = DataStatus.ToCreate;
            await _playerDataService.Create(player);
            _players.Add(player);
            Player_created?.Invoke(player);
        }
        public async Task UpdatePlayer(Player player)
        {
            _logger.LogInformation( "update player");
            if (player.DataStatus != DataStatus.ToCreate)
                player.DataStatus = DataStatus.ToUpdate;
            await _playerDataService.Update(player);
            int currentIndex = _players.FindIndex(y => y.Id == player.Id);

            if (currentIndex != -1)
            {
                _players[currentIndex] = player;
            }
            else
            {
                _players.Add(player);
            }
            Messenger.Default.Send(new EntityUpdated<Player>(player));
            Player_update?.Invoke(player);
        }
        public void UpdatePlayerBalance(int playerId,double value)
        {
            Player? player = _players.FirstOrDefault(x=>x.Id==playerId);
            if (player != null)
            {
                player.Balance += value;
                int currentIndex = _players.FindIndex(y => y.Id == player.Id);
                if (currentIndex != -1)
                {
                    _players[currentIndex] = player;
                }
                Player_update?.Invoke(_players[currentIndex]);
            }
           
        }
        public async Task DeletePlayer(int player)
        {
            _logger.LogInformation("force delete player");
            bool deleted = await _playerDataService.Delete(player);
            int currentIndex = _players.FindIndex(y => y.Id == player);
            _players.RemoveAt(currentIndex);
            Player_deleted?.Invoke(player);
        }
        public async Task<Player> GetPlayerById(int player_id)
        {
            _logger.LogInformation("force delete player");
            Player player = await _playerDataService.Get(player_id);
            return player;
        }
        private async Task Initialize()
        {
            _logger.LogInformation("init player");
            IEnumerable<Player> players = await _playerDataService.GetAll();
            _logger.LogInformation("sort players list");
            _players.Clear();
            _players.AddRange(players);
            Players_loaded?.Invoke();
        }
    }
   
}
