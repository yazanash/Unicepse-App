using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            sport.Trainers = data.Trainers.Select(x => new Core.Models.Employee.Employee { SyncId = x }).ToList();
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
            sportDto.Trainers = data.Trainers?.Select(x => x.SyncId).ToList()??new();
            return sportDto;
        }
    }
}
