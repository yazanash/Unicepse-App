using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.DataExporter.Dtos;

namespace Uniceps.DataExporter.Mappers
{
    public static class CreditMapper 
    {
        public static Credit FromDto(CreditDto data)
        {
            Credit credit = new Credit()
            {
                Date = data.Date,
                UpdatedAt = data.UpdatedAt,
                SyncId = data.SyncId,
                CreatedAt = data.CreatedAt,
                CreditValue = data.CreditValue,
                Description = data.Description,
                EmpPersonSyncId = data.EmpPersonSyncId,

            };
            return credit;
        }

        public static CreditDto ToDto(Credit data)
        {
            CreditDto creditdto = new CreditDto()
            {
                Date = data.Date,
                UpdatedAt = data.UpdatedAt,
                SyncId = data.SyncId,
                CreatedAt = data.CreatedAt,
                CreditValue = data.CreditValue,
                Description = data.Description,
                EmpPersonSyncId = data.EmpPersonSyncId,

            };
            return creditdto;
        }
    }
}
