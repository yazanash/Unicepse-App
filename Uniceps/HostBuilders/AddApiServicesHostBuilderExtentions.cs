using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.Exercises;
using Uniceps.API.Requests;
using Uniceps.API.Responses;
using Uniceps.API.SyncServices;
using Uniceps.BackgroundServices.SyncServices;
using Uniceps.BioEngineServices;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.SyncModel;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Services.DataSyncServices;

namespace Uniceps.HostBuilders
{
    public static class AddApiServicesHostBuilderExtentions
    {
        public static IHostBuilder AddApiServices(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<GetExercisesService>();
                services.AddSingleton<IDataService<SyncObject>, SyncDataService>();
                services.AddSingleton<IServerChanges<PushPlayerRequest,PullPlayerResponse>, PlayersSyncService>();
                services.AddSingleton<IPushPullDataService<Player>, PlayerLocalSyncDataService>();
                services.AddSingleton<ISyncEngine, PlayerSyncEngine>();
                services.AddSingleton<ISyncEngine, PlayerSyncEngine>();
            });
            return _hostBuilder;
        }
    }
    public static class AddBioEngineServicesHostBuilderExtentions
    {
        public static IHostBuilder AddBioEngine(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<DeviceScannerService>();
                services.AddSingleton<BiometricLogicService>();

                services.AddSingleton<BiometricStorageService>();
                services.AddSingleton<BioEngine>();
            });
            return _hostBuilder;
        }
    }
}
