using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SkiaSharp;
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
        public async Task<ApiResponse<List<ExerciseDtoModel>>> FetchExercises()
        {
            return await _client.GetAsync<List<ExerciseDtoModel>>($"ExerciseV2");
        }

        public async Task DownloadImage(string exerciseId, string localPath)
        {
            try
            {
                byte[] imageBytes = await _client.DownloadImage($"ExerciseV2/get-image/{exerciseId}");
                if (imageBytes == null || imageBytes.Length == 0) return;
                using (var bitmap = SKBitmap.Decode(imageBytes))
                {
                    if (bitmap == null) return;

                    using (var image = SKImage.FromBitmap(bitmap))
                    using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                    {
                        using (var stream = File.Create(localPath))
                        {
                            data.SaveTo(stream);
                        }
                    }
                }
            }
            catch { }
          
        }
        public async Task<ApiResponse<EssentialsReponse>> FetchEssentials()
        {
            return await _client.GetAsync<EssentialsReponse>($"ExerciseV2/GetEssentials");
        }

    }
}
