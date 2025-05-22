using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.utlis.LogEnrich
{
    public class AppVersionEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version!.ToString();
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("AppVersion", version));
        }
    }
}
