using Unicepse.Core.Models.Player;
using Unicepse.Entityframework.Services.PlayerQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.utlis.common;
using Unicepse.ViewModels.PlayersViewModels;
using Unicepse.API.Services;
using Unicepse.Core.Common;
using Unicepse.BackgroundServices;
using Unicepse.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Unicepse.Core.Services;
using Unicepse.Stores.ApiDataStores;

namespace Unicepse.Stores
{

    public class PlayersDataStore
    {
        private readonly IDataService<Player> _playerDataService;
        private readonly IArchivedService<Player> _archivedService;
        private readonly IPublicIdService<Player> _publicIdService;
        private readonly IApiDataStore<Player> _apiDataStore;
        private readonly PlayerApiDataService _playerApiDataService;
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
        public event Action<Profile>? profile_loaded;
        public event Action<Filter?>? FilterChanged;
        public event Action<Player?>? PlayerChanged;
        public PlayersDataStore(IDataService<Player> playerDataService, PlayerApiDataService playerApiDataService, ILogger<PlayersDataStore> logger, IArchivedService<Player> archivedService, IPublicIdService<Player> publicIdService, IApiDataStore<Player> apiDataStore)
        {
            _playerDataService = playerDataService;
            _archivedService = archivedService;
            _publicIdService = publicIdService;
            _players = new List<Player>();
            _archivedPlayers = new List<Player>();
            _initializeLazy = new Lazy<Task>(Initialize);
            _playerApiDataService = playerApiDataService;
            _logger = logger;
            _apiDataStore = apiDataStore;
        }

        internal async Task<Player?> GetPlayerByUID(string? uid)
        {
            _logger.LogInformation(LogFlag + "get player by id");
            Player? player = await _publicIdService.GetByUID(uid!);
            if (player == null)
            {
                _logger.LogError(LogFlag + "invalid Player Id");
                throw new NotExistException("هذا المستخدم لا يملك اي حساب مسجل");
            }
               
            return player;
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
        public async Task GetAll()
        {
            _logger.LogInformation(LogFlag + "get players");
            await _initializeLazy.Value;
            Players_loaded?.Invoke();
        }

        public async Task GetArchivedPlayers()
        {
            _logger.LogInformation(LogFlag + "get archived players");
            IEnumerable<Player> players = await _archivedService.GetAll();
            _archivedPlayers.Clear();
            _archivedPlayers.AddRange(players!);
            ArchivedPlayers_loaded?.Invoke();
        }
        public async Task HandShakePlayer(Player player,string uid)
        {
            _logger.LogInformation(LogFlag + "handshake player");
            Player? handSakePlayer = await _publicIdService.GetByUID(uid);
            if (handSakePlayer != null)
            {
                _logger.LogInformation(LogFlag + "handshake exists");
                throw new ConflictException("هذا المستخدم لديه حساب اخر موثق بالفعل ");
            }
               
            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            _logger.LogInformation(LogFlag + "check internet connection {0}", internetAvailable.ToString());
            if (internetAvailable)
            {
                try
                {
                    _logger.LogInformation(LogFlag + "add player to api");
                    int status = await _playerApiDataService.CreateHandShake(player, uid);
                    if (status == 201 || status == 409)
                    {
                        player.UID = uid;
                        _logger.LogInformation(LogFlag + "player handshake synced successfully with code {0}", status.ToString());
                        await _playerDataService.Update(player);
                        SelectedPlayer!.UID = player.UID;
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
                        //if(SelectedPlayer!=null && SelectedPlayer.Id == player.Id)
                        //{
                        //    SelectedPlayer.IsVerified = true;
                        //}
                    }
                    else
                    {
                        _logger.LogError(LogFlag + "handshake creation failed with code {0}", status.ToString());
                        throw new NotExistException("حساب هذه المستخدم غير متوفر لدينا يرجى التاكد من ان اللاعب قد قام بتحميل التطبيق الخاص بنا");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "player synced failed with error {0}", ex.Message);
                }

            }
            else
            {
                _logger.LogError(LogFlag + "failed to connect to internet");
                throw new Exception("لا يوجد اتصال بالانترنت");
            }
        }
        public async Task GetPlayerProfile(string uid)
        {
            Player? ExistedPlayer = await _publicIdService.GetByUID(uid);
            if (ExistedPlayer != null)
            {
                _logger.LogError(LogFlag + "hand shake is exists");
                throw new ConflictException("هذا المستخدم لديه حساب اخر موثق بالفعل ");

            }
            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            _logger.LogInformation(LogFlag + "check internet connection {0}", internetAvailable.ToString());
            if (internetAvailable)
            {
                try
                {
                    _logger.LogInformation(LogFlag + "get player profile from api");
                    Profile profile = await _playerApiDataService.GetProfile(uid);
                    if (profile != null)
                    {
                        profile_loaded?.Invoke(profile);
                    }
                }
                catch 
                {
                    _logger.LogError(LogFlag + "failed to valid player id ");
                    throw new Exception("خطا في التحقق من المستخدم");
                }
            }
            else
            {
                _logger.LogError(LogFlag + "failed to connect to internet");
                throw new Exception("لا يوجد اتصال بالانترنت");
            }
        }
        public async Task AddPlayer(Player player)
        {
            _logger.LogInformation(LogFlag + "add player");
            player.DataStatus = DataStatus.ToCreate;
            await _playerDataService.Create(player);
            await _apiDataStore.Create(player);
            _players.Add(player);
            Player_created?.Invoke(player);
        }
        public async Task UpdatePlayer(Player player)
        {
            _logger.LogInformation(LogFlag + "update player");
            if (player.DataStatus != DataStatus.ToCreate)
                player.DataStatus = DataStatus.ToUpdate;
            await _playerDataService.Update(player);
            await _apiDataStore.Update(player);
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
        public async Task DeletePlayer(Player player)
        {
            _logger.LogInformation(LogFlag + "delete player");
            await _playerDataService.Update(player);
            int currentIndex = _players.FindIndex(y => y.Id == player.Id);
            await _apiDataStore.Update(player);
            _players.RemoveAt(currentIndex);
            Player_deleted?.Invoke(player.Id);
            ArchivedPlayer_created?.Invoke(player);
        }
        public async Task ReactivePlayer(Player player)
        {
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
