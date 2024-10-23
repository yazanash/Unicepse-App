using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API.Models;

namespace Unicepse.API
{
    public class UnicepseApiPrepHttpClient
    {
        private readonly HttpClient _client;
        private readonly UnicepsePrepAPIKey _apiKey;
        public string? id => _apiKey.GymId;
        public UnicepseApiPrepHttpClient(HttpClient client, UnicepsePrepAPIKey apiKey)
        {
            _client = client;
            _apiKey = apiKey;
            //RunAsync();
        }
        //public async Task RunAsync()
        //{
        //    bool gender = true;
        //    PlayerDto entity = new PlayerDto()
        //    {
        //        balance = 0,
        //        date_of_birth = 1999,
        //        gender = gender.ToString(),
        //        gym_id = 18,
        //        height = 18.0,
        //        name = "yaz",
        //        phone_number = "+963994916917",
        //        pid = 22,
        //        width = 16.5
        //    };
        //    HttpContent content = new StringContent(JsonConvert.SerializeObject(entity));
        //    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    HttpResponseMessage response = await _client.PostAsync($"/player", content);
        //    string jsonResponse = await response.Content.ReadAsStringAsync();
        //}
        public async Task<T> GetAsync<T>(string uri)
        {
            if (!_client.DefaultRequestHeaders.Where(x => x.Key == ("x-access-token")).Any())
                _client.DefaultRequestHeaders.Add("x-access-token", _apiKey.Key);
            HttpResponseMessage response = await _client.GetAsync($"{uri}");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new Exception("هذه النسخة غير مفعلة");
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new Exception("هذا المعرف غير متوفر لدينا");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonResponse)!;
        }

        public async Task<byte[]?> GetByteArrayAsync(string uri)
        {
            try
            {
                byte[] logo = await _client.GetByteArrayAsync($"{uri}");
                return logo;
            }
            catch
            {
                return null;
            }
        }
        public async Task<int> GetCodeAsync<T>(string uri)
        {
            if (!_client.DefaultRequestHeaders.Where(x => x.Key == ("x-access-token")).Any())
                _client.DefaultRequestHeaders.Add("x-access-token", _apiKey.Key);
            HttpResponseMessage response = await _client.GetAsync($"{uri}");
            return ((int)response.StatusCode);
        }
        public async Task<int> PostAsync<T>(string uri, T entity)
        {
            if (!_client.DefaultRequestHeaders.Where(x => x.Key == ("x-access-token")).Any())
                _client.DefaultRequestHeaders.Add("x-access-token", _apiKey.Key);
            HttpContent content = new StringContent(JsonConvert.SerializeObject(entity));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await _client.PostAsync($"{uri}", content);
            string data = await response.Content.ReadAsStringAsync();
            return ((int)response.StatusCode);

        }
        public async Task<int> PutAsync<T>(string uri, T entity)
        {
            if (!_client.DefaultRequestHeaders.Where(x => x.Key == ("x-access-token")).Any())
                _client.DefaultRequestHeaders.Add("x-access-token", _apiKey.Key);
            HttpContent content = new StringContent(JsonConvert.SerializeObject(entity));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await _client.PutAsync($"{uri}", content);
            return ((int)response.StatusCode);

        }
        public async Task<int> DeleteAsync<T>(string uri)
        {
            if (!_client.DefaultRequestHeaders.Where(x => x.Key == ("x-access-token")).Any())
                _client.DefaultRequestHeaders.Add("x-access-token", _apiKey.Key);
            HttpResponseMessage response = await _client.DeleteAsync($"{uri}");
            return ((int)response.StatusCode);

        }
    }
}
