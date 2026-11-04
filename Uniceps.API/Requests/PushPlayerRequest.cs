using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;

namespace Uniceps.API.Requests
{
    public class PushPlayerRequest
    {
        public PushPlayerRequest(Player player)
        {
            CreatedAt = player.CreatedAt;
            UpdatedAt = player.UpdatedAt;
            SyncId = player.SyncId;
            FullName = player.FullName ?? "";
            Phone = player.Phone ?? "";
            BirthDate = player.BirthDate;
            GenderMale = player.GenderMale;
            FingerprintData = player.FingerprintData ?? "";
            MediclStatus = player.MediclStatus ?? "";
        }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Guid SyncId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int BirthDate { get; set; }
        public bool GenderMale { get; set; }
        public string FingerprintData { get; set; } = string.Empty;
        public string MediclStatus { get; set; } = string.Empty;

    }
}
