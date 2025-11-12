using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.API;
using Uniceps.API.Models;

namespace Uniceps.HostBuilders
{
    public static class AddHttpClientHostBuilderExtentions
    {
        public static IHostBuilder AddHttpClient(this IHostBuilder _hostBuilder)
        {
            string? apiUrl = ConfigurationManager.AppSettings["ApiUrl"];

            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton(new UnicepsePrepAPIKey(""));
                services.AddHttpClient<UnicepseApiPrepHttpClient>(c =>
                {
                    c.Timeout = TimeSpan.FromSeconds(10);

                    c.BaseAddress = new Uri(apiUrl!);

                });
                services.AddHttpClient<UnicepseApiClientV2>(c =>
                {
                    c.Timeout = TimeSpan.FromSeconds(10);

                    c.BaseAddress = new Uri(apiUrl!);

                });
            });
            return _hostBuilder;
        }
    }
}
