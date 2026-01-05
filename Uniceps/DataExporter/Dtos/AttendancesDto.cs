using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.DataExporter.Dtos
{
    public class AttendancesDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid SyncId { get; set; }
        public DateTime Date { get; set; }
        public DateTime loginTime { get; set; }
        public DateTime logoutTime { get; set; }
        public int PlayerId { get; set; }
        public Guid PlayerSyncId { get; set; }
        public int SubscriptionId { get; set; }
        public Guid SubscriptionSyncId { get; set; }
        public string Code { get; set; } = "";
        public string PlayerName { get; set; } = "";
        public string SportName { get; set; } = "";
        public int KeyNumber { get; set; }
        public bool IsTakenKey { get; set; }
        public bool IsLogged { get; set; }
    }
}
