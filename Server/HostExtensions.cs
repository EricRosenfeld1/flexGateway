using flexGateway.Common.Adapter;
using flexGateway.Common.MachineNode;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using flexGateway.Common.AdapterNode;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace flexGateway.Server
{
    public static class HostExtension
    {
        public static IHost LoadPlugins(this IHost host)
        {
            var pluginManager = new PluginManager(
                host.Services.GetService<IAdapterFactory>(),
                host.Services.GetService<INodeFactory>());
            pluginManager.LoadPlugins(host.Services.GetService<ILogger<PluginManager>>());

            return host;
        }
    }
}
