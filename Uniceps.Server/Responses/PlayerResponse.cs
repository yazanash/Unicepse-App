using Uniceps.Core.Models.Player;

namespace Uniceps.Server.Responses
{
    public class PlayerResponse
    {
        public PlayerResponse(Player player)
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
            ServerVersion = player.ServerVersion;
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
        public long ServerVersion { get; set; }
    }
}
