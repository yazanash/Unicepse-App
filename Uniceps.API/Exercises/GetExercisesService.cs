using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.Models;
using ex = Uniceps.Core.Models.TrainingProgram;

namespace Uniceps.API.Exercises
{
    public class GetExercisesService
    {
        public async Task<List<ExerciseDtoModel>> FetchExercises()
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://yazanash-001-site1.anytempurl.com/api/Exercise";
                string response = await client.GetStringAsync(apiUrl);
                List<ExerciseDtoModel> exercises = JsonConvert.DeserializeObject<List<ExerciseDtoModel>>(response)!;

                return exercises;
            }
        }

        public async Task DownloadImage(string imageUrl, string localPath)
        {
            using (HttpClient client = new HttpClient())
            {
                byte[] imageBytes = await client.GetByteArrayAsync(imageUrl);
                await File.WriteAllBytesAsync(localPath, imageBytes);
            }
        }
        public async Task<List<MuscleGroupDto>> FetchMuscleGroup()
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://yazanash-001-site1.anytempurl.com/api/MuscleGroup";
                string response = await client.GetStringAsync(apiUrl);
                List<MuscleGroupDto> exercises = JsonConvert.DeserializeObject<List<MuscleGroupDto>>(response)!;

                return exercises;
            }
        }

    }
}
