using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Commands.Player;

namespace Unicepse.HostBuilders
{
    public static class AddCommandsHostBuilderExtentions
    {
        public static IHostBuilder AddCommands(this IHostBuilder _hostBuilder)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<UpdateCurrentViewModelCommand>();
            });
            return _hostBuilder;
        }
    }
}
