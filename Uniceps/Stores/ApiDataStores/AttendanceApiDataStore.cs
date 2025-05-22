using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.Models;
using Uniceps.API.Services;
using Uniceps.BackgroundServices;
using Uniceps.Core.Common;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Models.SyncModel;
using Uniceps.Core.Services;

namespace Uniceps.Stores.ApiDataStores
{
    public class AttendanceApiDataStore : IApiDataStore<DailyPlayerReport>
    {
        private readonly AttendanceApiDataService _playersAttendenceApiService;
        private readonly IDataService<SyncObject> _syncStore;
        private readonly ILogger<IApiDataStore<DailyPlayerReport>> _logger;
        string LogFlag = "[Attendence] ";

        public AttendanceApiDataStore(AttendanceApiDataService playersAttendenceApiService, ILogger<IApiDataStore<DailyPlayerReport>> logger, IDataService<SyncObject> syncStore)
        {
            _playersAttendenceApiService = playersAttendenceApiService;
            _logger = logger;
            _syncStore = syncStore;
        }

        public async Task Create(DailyPlayerReport entity)
        {
            AttendanceDto attendanceDto = new AttendanceDto();
            attendanceDto.FromAttendance(entity);
            SyncObject syncObject = new SyncObject()
            {
                OperationType = DataStatus.ToCreate,
                EntityType = DataType.Attendance,
                ObjectData = JsonConvert.SerializeObject(attendanceDto)
            };
            await _syncStore.Create(syncObject);
        }

        public async Task Sync(SyncObject syncObject)
        {
            bool internetAvailable = InternetAvailability.IsInternetAvailable();
            _logger.LogInformation(LogFlag + "check internet connection {0}", internetAvailable.ToString());
            if (internetAvailable)
            {
                try
                {
                    AttendanceDto? attendanceDto = JsonConvert.DeserializeObject<AttendanceDto>(syncObject.ObjectData!);
                    if (attendanceDto != null)
                    {
                        if (syncObject.OperationType == DataStatus.ToCreate)
                        {
                            _logger.LogInformation(LogFlag + "add attendance to api");

                            int status = await _playersAttendenceApiService.Create(attendanceDto);
                            if (status == 201 || status == 409)
                            {
                                _logger.LogInformation(LogFlag + "attendance synced successfully with code {0}", status.ToString());
                                await _syncStore.Delete(syncObject.Id);
                            }
                        }
                        else
                        {
                            _logger.LogInformation(LogFlag + "update attendance to api");

                            int status = await _playersAttendenceApiService.Update(attendanceDto);
                            if (status == 200)
                            {
                                _logger.LogInformation(LogFlag + "attendance synced successfully with code {0}", status.ToString());
                                await _syncStore.Delete(syncObject.Id);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(LogFlag + "attendance synced failed with error {0}", ex.Message);
                }
            }
        }

        public async Task Update(DailyPlayerReport entity)
        {
            AttendanceDto attendanceDto = new AttendanceDto();
            attendanceDto.FromAttendance(entity);
            SyncObject syncObject = new SyncObject()
            {
                OperationType = DataStatus.ToUpdate,
                EntityType = DataType.Attendance,
                ObjectData = JsonConvert.SerializeObject(attendanceDto)
            };
            await _syncStore.Create(syncObject);
        }
    }
}
