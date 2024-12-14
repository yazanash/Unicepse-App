using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Common;

namespace Unicepse.Core.Models.SyncModel
{
    public class SyncObject :DomainObject
    {
        public DataStatus OperationType { get; set; }
        public DataType EntityType { get; set; }
        public string? ObjectData { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
