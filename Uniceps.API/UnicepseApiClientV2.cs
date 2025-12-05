using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.common;
using Uniceps.API.Models;

namespace Uniceps.API
{
    public class UnicepseApiClientV2
    {
        private readonly HttpClient _client;
        private readonly UnicepsePrepAPIKey _apiKey;
        public UnicepseApiClientV2(HttpClient client, UnicepsePrepAPIKey apiKey)
        {
            _client = client;
            _apiKey = apiKey;
            _client = client;
            _apiKey = apiKey;
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                if (!string.IsNullOrEmpty(_apiKey.Key))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey.Key);
                }
            }
        }
        private void EnsureAuthHeader()
        {
            if (!_client.DefaultRequestHeaders.Contains("Authorization"))
            {
                if (!string.IsNullOrEmpty(_apiKey.Key))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey.Key);
                }
            }
        }
        public async Task<ApiResponse<TResult>> PostAsync<TRequest, TResult>(string uri, TRequest entity)
        {
            EnsureAuthHeader();

            var jsonContent = JsonConvert.SerializeObject(entity);
            HttpContent content = new StringContent(jsonContent);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _client.PostAsync(uri, content);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = new ApiResponse<TResult>
            {
                StatusCode = (int)response.StatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    apiResponse.Data = JsonConvert.DeserializeObject<TResult>(jsonResponse);

                }
                catch
                {

                }
            }
            else
            {
                apiResponse.ErrorMessage = jsonResponse;
            }

            return apiResponse;
        }
        public async Task<ApiResponse<TResult>> PostPictureAsync<TRequest, TResult>(string uri, TRequest entity)
        {
            EnsureAuthHeader();

            var apiResponse = new ApiResponse<TResult>();
            string? filePath = entity?.ToString();
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                apiResponse.StatusCode = 400;
                apiResponse.ErrorMessage = "File does not exist.";
                return apiResponse;
            }

            using (var content = new MultipartFormDataContent())
            {
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                content.Add(fileContent, "file", Path.GetFileName(filePath));

                HttpResponseMessage response = await _client.PostAsync(uri, content);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                apiResponse.StatusCode = (int)response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // نتوقع JSON بالشكل { "imageUrl": "اسم_الصورة.jpg" }
                        apiResponse.Data = JsonConvert.DeserializeObject<TResult>(jsonResponse);
                    }
                    catch (Exception ex)
                    {
                        apiResponse.ErrorMessage = "Failed to parse response: " + ex.Message;
                    }
                }
                else
                {
                    apiResponse.ErrorMessage = jsonResponse;
                }

                fileStream.Dispose();
            }

            return apiResponse;
        }

        public async Task<ApiResponse<TResult>> GetAsync<TResult>(string uri)
        {
            EnsureAuthHeader();

            HttpResponseMessage response = await _client.GetAsync(uri);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = new ApiResponse<TResult>
            {
                StatusCode = (int)response.StatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                apiResponse.Data = JsonConvert.DeserializeObject<TResult>(jsonResponse);
            }
            else
            {
                apiResponse.ErrorMessage = jsonResponse;
            }

            return apiResponse;
        }
        public async Task<ApiResponse<byte[]>> GetByteArrayAsync(string uri)
        {
            EnsureAuthHeader();

            HttpResponseMessage response = await _client.GetAsync(uri);
            var apiResponse = new ApiResponse<byte[]>
            {
                StatusCode = (int)response.StatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                apiResponse.Data = await response.Content.ReadAsByteArrayAsync();
            }
            else
            {
                apiResponse.ErrorMessage = await response.Content.ReadAsStringAsync();
            }

            return apiResponse;
        }
        public async Task<ApiResponse<TResult>> PutAsync<TRequest, TResult>(string uri, TRequest entity)
        {
            EnsureAuthHeader();

            var jsonContent = JsonConvert.SerializeObject(entity);
            HttpContent content = new StringContent(jsonContent);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _client.PutAsync(uri, content);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = new ApiResponse<TResult>
            {
                StatusCode = (int)response.StatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                apiResponse.Data = JsonConvert.DeserializeObject<TResult>(jsonResponse);
            }
            else
            {
                apiResponse.ErrorMessage = jsonResponse;
            }

            return apiResponse;
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string uri)
        {
            EnsureAuthHeader();

            HttpResponseMessage response = await _client.DeleteAsync(uri);

            var apiResponse = new ApiResponse<bool>
            {
                StatusCode = (int)response.StatusCode,
                Data = response.IsSuccessStatusCode
            };

            if (!response.IsSuccessStatusCode)
            {
                apiResponse.ErrorMessage = await response.Content.ReadAsStringAsync();
            }

            return apiResponse;
        }
    }
}
