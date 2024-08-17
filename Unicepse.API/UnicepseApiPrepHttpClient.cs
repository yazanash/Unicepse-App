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
        private readonly string _apiKey;

        public UnicepseApiPrepHttpClient(HttpClient client, UnicepsePrepAPIKey apiKey)
        {
            _client = client;
            _apiKey = apiKey.Key;
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
            HttpResponseMessage response = await _client.GetAsync($"{uri}");
            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonResponse)!;
        }
        public async Task<bool> PostAsync<T>(string uri, T entity)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(entity));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await _client.PostAsync($"{uri}", content);
            return response.StatusCode == HttpStatusCode.Created;

        }
        public async Task<bool> PutAsync<T>(string uri, T entity)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(entity));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await _client.PutAsync($"{uri}", content);
            return response.StatusCode == HttpStatusCode.OK;

        }
    }
}
