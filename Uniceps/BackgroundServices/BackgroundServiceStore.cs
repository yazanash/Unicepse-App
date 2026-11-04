using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Uniceps.BackgroundServices.SyncServices;
using Uniceps.Core.Common;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Models.Metric;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Models.SyncModel;
using Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.BackgroundServices
{
    public class BackgroundServiceStore
    {
        private readonly ILogger<BackgroundServiceStore> _logger;
        private readonly IEnumerable<ISyncEngine> _syncEngines;
        public BackgroundServiceStore(ILogger<BackgroundServiceStore> logger, IEnumerable<ISyncEngine> syncEngines)
        {
            _logger = logger;
            _syncEngines = syncEngines.OrderBy(e => e.Priority);
        }
        private string? _backMessage;
        public string? BackMessage
        {
            get { return _backMessage; }
            set { _backMessage = value; }
        }
        private bool _connection;
        public bool Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }
        private bool _syncStateProp;
        public bool SyncStateProp
        {
            get { return _syncStateProp; }
            set { _syncStateProp = value; }
        }
        private string? _syncMessage;
        public string? SyncMessage
        {
            get { return _syncMessage; }
            set { _syncMessage = value; }
        }
        public event Action<string?, bool>? StateChanged;
        public event Action<bool, string?>? SyncStatus;

        public void ChangeState(string message, bool connectionStatus)
        {
            BackMessage = message;
            Connection = connectionStatus;
            StateChanged?.Invoke(message, connectionStatus);
        }
        public void SyncState(bool connectionStatus, string message)
        {
            SyncMessage = message;
            SyncStateProp = connectionStatus;
            SyncStatus?.Invoke(connectionStatus, message);
        }
        public async Task Sync(CancellationToken cancellationToken = default)
        {
            foreach (var engine in _syncEngines)
            {
                try
                {
                    _logger.LogInformation("بدء {EngineName}...", engine.Name);

                    await engine.SyncAsync(cancellationToken);

                    _logger.LogInformation("تمت {EngineName} بنجاح.", engine.Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "فشلت {EngineName}!", engine.Name);

                    if (engine.Priority == 1)
                    {
                        _logger.LogWarning("إيقاف المزامنة: فشلت البيانات الأساسية التي تعتمد عليها بقية الجداول.");
                        break;
                    }
                }
            }
        }
    }
}
