using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.Models;
using Uniceps.API.Services;
using Uniceps.BackgroundServices;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.SyncModel;
using Uniceps.Core.Services;

namespace Uniceps.Stores.ApiDataStores
{
    public class PlayerApiDataStore : IApiDataStore<Player>
    {
        private readonly PlayerApiDataService _playerApiDataService;
        private readonly IDataService<SyncObject> _syncStore;
        private readonly ILogger<IApiDataStore<Player>> _logger;
        string LogFlag = "[PADS] ";
        public PlayerApiDataStore(PlayerApiDataService playerApiDataService, ILogger<IApiDataStore<Player>> logger, IDataService<SyncObject> syncStore)
        {
            _playerApiDataService = playerApiDataService;
            _logger = logger;
            _syncStore = syncStore;
        }

        public async Task Create(Player player)
        {
            PlayerDto playerDto = new PlayerDto();
            playerDto.FromPlayer(player);
            SyncObject syncObject = new SyncObject()
            {
                OperationType = DataStatus.ToCreate,
                EntityType = DataType.Player,
                ObjectData = JsonConvert.SerializeObject(playerDto)
            };
            await _syncStore.Create(syncObject);
        }

        public async Task Sync(SyncObject syncObject)
        {
            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            _logger.LogInformation(LogFlag + "check internet connection {0}", internetAvailable.ToString());
            if (internetAvailable)
            {
                try
                {
                    PlayerDto? playerDto = JsonConvert.DeserializeObject<PlayerDto>(syncObject.ObjectData!);
                    if (playerDto != null)
                    {
                        if (syncObject.OperationType == DataStatus.ToCreate)
                        {
                            _logger.LogInformation(LogFlag + "add player to api");

                            int status = await _playerApiDataService.Create(playerDto);
                            if (status == 201 || status == 409)
                            {
                                _logger.LogInformation(LogFlag + "player synced successfully with code {0}", status.ToString());
                                await _syncStore.Delete(syncObject.Id);
                            }
                        }
                        else
                        {
                            _logger.LogInformation(LogFlag + "update player to api");

                            int status = await _playerApiDataService.Update(playerDto);
                            if (status == 200)
                            {
                                _logger.LogInformation(LogFlag + "player synced successfully with code {0}", status.ToString());
                                await _syncStore.Delete(syncObject.Id);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "update synced failed with error {0}", ex.Message);
                }
            }
        }

        public async Task Update(Player player)
        {
            PlayerDto playerDto = new PlayerDto();
            playerDto.FromPlayer(player);
            SyncObject syncObject = new SyncObject()
            {
                OperationType = DataStatus.ToUpdate,
                EntityType = DataType.Player,
                ObjectData = JsonConvert.SerializeObject(playerDto)
            };
            await _syncStore.Create(syncObject);
        }
    }
}
