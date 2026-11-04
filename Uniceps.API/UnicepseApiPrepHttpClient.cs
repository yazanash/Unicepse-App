using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.common;
using Uniceps.API.Models;
using Uniceps.Core.Exceptions;

namespace Uniceps.API
{
    public class UnicepseApiPrepHttpClient
    {
        private readonly HttpClient _client;
        public UnicepseApiPrepHttpClient(HttpClient client)
        {
            _client = client;

        }
        /// <summary>
        /// Check Internet Status
        /// </summary>
        public async Task<bool> CheckConnectionAsync(string uri = "health")
        {
            try
            {
                using var response = await _client.GetAsync(uri);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        ///  Get Data
        /// </summary>
        public async Task<ApiResponse<TResult>> GetAsync<TResult>(string uri)
        {
            try
            {
                var response = await _client.GetAsync(uri);
                var result = new ApiResponse<TResult> { StatusCode = (int)response.StatusCode };

                if (response.IsSuccessStatusCode)
                {
                    result.Data = await response.Content.ReadFromJsonAsync<TResult>();
                }
                else
                {
                    result.ErrorMessage = await response.Content.ReadAsStringAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                return new ApiResponse<TResult> { StatusCode = 503, ErrorMessage = $"Network Error: {ex.Message}" };
            }
        }

        /// <summary>
        /// إرسال بيانات JSON (Post)
        /// </summary>
        public async Task<ApiResponse<TResult>> PostAsync<TRequest, TResult>(string uri, TRequest entity)
        {
            try
            {
                var response = await _client.PostAsJsonAsync(uri, entity);
                var result = new ApiResponse<TResult> { StatusCode = (int)response.StatusCode };

                if (response.IsSuccessStatusCode)
                {
                    result.Data = await response.Content.ReadFromJsonAsync<TResult>();
                }
                else
                {
                    result.ErrorMessage = await response.Content.ReadAsStringAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                return new ApiResponse<TResult> { StatusCode = 503, ErrorMessage = $"Network Error: {ex.Message}" };
            }
        }

        /// <summary>
        /// رفع صورة أو ملف (Post Picture)
        /// </summary>
        public async Task<ApiResponse<TResult>> PostPictureAsync<TResult>(string uri, string filePath)
        {
            var apiResponse = new ApiResponse<TResult>();

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                apiResponse.StatusCode = 400;
                apiResponse.ErrorMessage = "File does not exist.";
                return apiResponse;
            }

            try
            {
                using var content = new MultipartFormDataContent();
                await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                content.Add(fileContent, "file", Path.GetFileName(filePath));

                var response = await _client.PostAsync(uri, content);
                apiResponse.StatusCode = (int)response.StatusCode;

                if (response.IsSuccessStatusCode)
                {
                    apiResponse.Data = await response.Content.ReadFromJsonAsync<TResult>();
                }
                else
                {
                    apiResponse.ErrorMessage = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = 503;
                apiResponse.ErrorMessage = $"Upload Error: {ex.Message}";
            }

            return apiResponse;
        }

        /// <summary>
        /// تحديث بيانات (Put)
        /// </summary>
        public async Task<ApiResponse<TResult>> PutAsync<TRequest, TResult>(string uri, TRequest entity)
        {
            try
            {
                var response = await _client.PutAsJsonAsync(uri, entity);
                var result = new ApiResponse<TResult> { StatusCode = (int)response.StatusCode };

                if (response.IsSuccessStatusCode)
                {
                    result.Data = await response.Content.ReadFromJsonAsync<TResult>();
                }
                else
                {
                    result.ErrorMessage = await response.Content.ReadAsStringAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                return new ApiResponse<TResult> { StatusCode = 503, ErrorMessage = $"Network Error: {ex.Message}" };
            }
        }

        /// <summary>
        /// حذف عنصر (Delete)
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteAsync(string uri)
        {
            try
            {
                var response = await _client.DeleteAsync(uri);
                return new ApiResponse<bool>
                {
                    StatusCode = (int)response.StatusCode,
                    Data = response.IsSuccessStatusCode,
                    ErrorMessage = response.IsSuccessStatusCode ? null : await response.Content.ReadAsStringAsync()
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool> { StatusCode = 503, Data = false, ErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// تحميل صورة أو ملف كـ Bytes
        /// </summary>
        public async Task<byte[]?> DownloadImageAsync(string imageUrl)
        {
            try
            {
                return await _client.GetByteArrayAsync(imageUrl);
            }
            catch
            {
                return null;
            }
        }

    }
}
