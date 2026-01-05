using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.DataExporter.Dtos;

namespace Uniceps.DataExporter.Mappers
{
    public static class AttendanceMapper 
    {
        public static DailyPlayerReport FromDto(AttendancesDto dto)
        {
            DailyPlayerReport dailyPlayerReport = new DailyPlayerReport()
            {
                Date = dto.Date,
                loginTime = dto.loginTime,
                logoutTime = dto.logoutTime,
                PlayerId = dto.PlayerId,
                PlayerSyncId = dto.PlayerSyncId,
                SubscriptionId = dto.SubscriptionId,
                SubscriptionSyncId = dto.SubscriptionSyncId,
                Code = dto.Code,
                PlayerName = dto.PlayerName,
                SportName = dto.SportName,
                KeyNumber = dto.KeyNumber,
                IsTakenKey = dto.IsTakenKey,
                IsLogged = dto.IsLogged,
                CreatedAt = dto.CreatedAt,
                SyncId = dto.SyncId,
                UpdatedAt = dto.UpdatedAt,
            };
            return dailyPlayerReport;
        }

        public static AttendancesDto ToDto(DailyPlayerReport data)
        {
            AttendancesDto attendancesDto = new AttendancesDto()
            {
                Date = data.Date,
                loginTime = data.loginTime,
                logoutTime = data.logoutTime,
                PlayerId = data.PlayerId,
                PlayerSyncId = data.PlayerSyncId,
                SubscriptionId = data.SubscriptionId,
                SubscriptionSyncId = data.SubscriptionSyncId,
                Code = data.Code,
                PlayerName = data.PlayerName,
                SportName = data.SportName,
                KeyNumber = data.KeyNumber,
                IsTakenKey = data.IsTakenKey,
                IsLogged = data.IsLogged,
                CreatedAt = data.CreatedAt,
                SyncId = data.SyncId,
                UpdatedAt = data.UpdatedAt,
            };
            return attendancesDto;
        }
    }
}
