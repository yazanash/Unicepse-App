using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;

namespace Uniceps.Core.Models.DailyActivity
{
    public class DailyPlayerReport : DomainObject
    {

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
        public DataStatus DataStatus { get; set; }

        public void MergeWith(DailyPlayerReport dailyPlayerReport)
        {
            Date = dailyPlayerReport.Date;
            loginTime = dailyPlayerReport.loginTime;
            logoutTime = dailyPlayerReport.logoutTime;
            PlayerId = dailyPlayerReport.PlayerId;
            PlayerSyncId = dailyPlayerReport.PlayerSyncId;
            SubscriptionId = dailyPlayerReport.SubscriptionId;
            SubscriptionSyncId = dailyPlayerReport.SubscriptionSyncId;
            Code = dailyPlayerReport.Code;
            PlayerName = dailyPlayerReport.PlayerName;
            SportName = dailyPlayerReport.SportName;
            KeyNumber = dailyPlayerReport.KeyNumber;
            IsTakenKey = dailyPlayerReport.IsTakenKey;
            IsLogged = dailyPlayerReport.IsLogged;
        }
    }
}
