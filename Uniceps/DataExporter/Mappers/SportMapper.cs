using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Sport;
using Uniceps.DataExporter.Dtos;

namespace Uniceps.DataExporter.Mappers
{
    public static class SportMapper 
    {
        public static Sport FromDto(SportDto data)
        {
            Sport sport = new Sport
            {
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                SyncId = data.SyncId,
                Name = data.Name,
                Price = data.Price,
                IsActive = data.IsActive,
                DaysInWeek = data.DaysInWeek,
                DaysCount = data.DaysCount,
            };
            return sport;
        }

        public static SportDto ToDto(Sport data)
        {
            SportDto sportDto = new SportDto
            {
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                SyncId = data.SyncId,
                Name = data.Name,
                Price = data.Price,
                IsActive = data.IsActive,
                DaysInWeek = data.DaysInWeek,
                DaysCount = data.DaysCount,
            };
            return sportDto;
        }
    }
}
