using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API;
using Unicepse.API.Models;

namespace Unicepse.HostBuilders
{
    public static class AddHttpClientHostBuilderExtentions
    {
        public static IHostBuilder AddHttpClient(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton(new UnicepsePrepAPIKey(""));
                services.AddHttpClient<UnicepseApiPrepHttpClient>(c =>
                {
                    c.Timeout = TimeSpan.FromSeconds(10);
                    //c.BaseAddress = new Uri("https://uniceps.trio-verse.com/api/v1/");
                    c.BaseAddress = new Uri("https://uniapi-ui65lw0m.b4a.run/api/v1/");
                });
            });
            return _hostBuilder;
        }
    }
}
