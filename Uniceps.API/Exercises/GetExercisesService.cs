using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.common;
using Uniceps.API.Models;

namespace Uniceps.API.Exercises
{
    public class GetExercisesService
    {
        private readonly UnicepseApiClientV2 _client;
        public GetExercisesService(UnicepseApiClientV2 client)
        {
            _client = client;
        }
        public async Task<ApiResponse<List<ExerciseDtoModel>>> FetchExercises(int id)
        {
            return await _client.GetAsync<List<ExerciseDtoModel>>($"Exercise?id={id}");
        }

        public async Task DownloadImage(string imageUrl, string localPath)
        {
            using (HttpClient client = new HttpClient())
            {
                byte[] imageBytes = await client.GetByteArrayAsync(imageUrl);
                await File.WriteAllBytesAsync(localPath, imageBytes);
            }
        }
        public async Task<ApiResponse<List<MuscleGroupDto>>> FetchMuscleGroup()
        {
            return await _client.GetAsync<List<MuscleGroupDto>>($"MuscleGroup");
        }

    }
}
