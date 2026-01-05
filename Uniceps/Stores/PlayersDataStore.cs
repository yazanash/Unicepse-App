using Uniceps.Entityframework.Services.PlayerQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Uniceps.utlis.common;
using Uniceps.Stores.ApiDataStores;
using Uniceps.BackgroundServices;
using Uniceps.Core.Services;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Common;
using Uniceps.API.Services;
using Uniceps.Services;

namespace Uniceps.Stores
{

    public class PlayersDataStore
    {
        private readonly IDataService<Player> _playerDataService;
        private readonly IArchivedService<Player> _archivedService;
        private readonly AccountStore _accountStore;
        private readonly IExcelService<Player> _excelService;
        private readonly List<Player> _players;
        private readonly List<Player> _archivedPlayers;
        private readonly Lazy<Task> _initializeLazy;
        string LogFlag = "[Players] ";
        private readonly ILogger<PlayersDataStore> _logger;

        public IEnumerable<Player> Players => _players;
        public IEnumerable<Player> ArchivedPlayers => _archivedPlayers;
        public event Action<Player>? Player_created;
        public event Action? Players_loaded;
        public event Action<Player>? Player_update;
        public event Action<int>? Player_deleted;
        public event Action? ArchivedPlayers_loaded;
        public event Action<Filter?>? FilterChanged;
        public event Action<Player?>? PlayerChanged;
        public PlayersDataStore(IDataService<Player> playerDataService,  ILogger<PlayersDataStore> logger, IArchivedService<Player> archivedService,  IExcelService<Player> excelService, AccountStore accountStore)
        {
            _playerDataService = playerDataService;
            _archivedService = archivedService;
            _players = new List<Player>();
            _archivedPlayers = new List<Player>();
            _initializeLazy = new Lazy<Task>(Initialize);
            _logger = logger;
            _excelService = excelService;
            _accountStore = accountStore;
        }

        private Player? _selectedPlayer;
        public Player? SelectedPlayer
        {
            get
            {
                return _selectedPlayer;
            }
            set
            {
                _selectedPlayer = value;
                PlayerChanged?.Invoke(_selectedPlayer);
            }
        }

        private Filter? _selectedFilter;
        public Filter? SelectedFilter
        {
            get
            {
                return _selectedFilter;
            }
            set
            {
                _selectedFilter = value;
                FilterChanged?.Invoke(_selectedFilter);
            }
        }


        private Order? _selectedOrder;
        public Order? SelectedOrder
        {
            get
            {
                return _selectedOrder;
            }
            set
            {
                _selectedOrder = value;
                OrderChanged?.Invoke(_selectedOrder);
            }
        }


        public event Action<Player>? ArchivedPlayer_created;
        public event Action<Player>? ArchivedPlayer_restored;

        public event Action<Order?>? OrderChanged;

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
            _logger.LogInformation(LogFlag + "get players");
            await _initializeLazy.Value;
            Players_loaded?.Invoke();
        }

        public async Task GetArchivedPlayers()
        {
            _logger.LogInformation(LogFlag + "get archived players");
            IEnumerable<Player> players = await _archivedService.GetAllArchived();
            _archivedPlayers.Clear();
            _archivedPlayers.AddRange(players!);
            ArchivedPlayers_loaded?.Invoke();
        }
       
        public async Task AddPlayer(Player player)
        {
            if (_accountStore.SystemSubscription == null && _players.Count() + _archivedPlayers.Count() >= 50)
                throw new FreeLimitException("لقد وصلت الحد الاعلى من النسخة المجانية ... اشترك الان لتحصل عدد غير محدود");

                _logger.LogInformation(LogFlag + "add player");
            player.DataStatus = DataStatus.ToCreate;
            await _playerDataService.Create(player);
            _players.Add(player);
            Player_created?.Invoke(player);
        }
        public async Task UpdatePlayer(Player player)
        {
            _logger.LogInformation(LogFlag + "update player");
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
        public void UpdatePlayerDate(int playerId, DateTime value)
        {
            Player? player = _players.FirstOrDefault(x => x.Id == playerId);
            if (player != null)
            {
                if (player.SubscribeEndDate<=value)
                player.SubscribeEndDate = value;
                int currentIndex = _players.FindIndex(y => y.Id == player.Id);
                if (currentIndex != -1)
                {
                    _players[currentIndex] = player;
                }
                Player_update?.Invoke(_players[currentIndex]);
            }

        }
        public async Task DeletePlayer(Player player)
        {
            _logger.LogInformation(LogFlag + "delete player");
            await _playerDataService.Update(player);
            int currentIndex = _players.FindIndex(y => y.Id == player.Id);
            _players.RemoveAt(currentIndex);
            Player_deleted?.Invoke(player.Id);
            ArchivedPlayer_created?.Invoke(player);
        }
        public async Task ReactivePlayer(Player player)
        {
            if (_accountStore.SystemSubscription == null && _players.Count() + _archivedPlayers.Count() >= 50)
                throw new Exception("لقد وصلت الحد الاعلى من النسخة المجانية ... اشترك الان لتحصل عدد غير محدود");

            _logger.LogInformation(LogFlag + "reactive player");
            await _playerDataService.Update(player);
            _players.Add(player);
            ArchivedPlayer_restored?.Invoke(player);
        }
        public async Task ForceDeletePlayer(int player_id)
        {
            _logger.LogInformation(LogFlag + "force delete player");
            bool deleted = await _playerDataService.Delete(player_id);
            int currentIndex = _players.FindIndex(y => y.Id == player_id);
            _players.RemoveAt(currentIndex);
            Player_deleted?.Invoke(player_id);
        }


        private async Task Initialize()
        {
            _logger.LogInformation(LogFlag + "init player");
            IEnumerable<Player> players = await _playerDataService.GetAll();
            _logger.LogInformation(LogFlag + "sort players list");
            _players.Clear();
            _players.AddRange(players);
            Players_loaded?.Invoke();
        }
    }
}
