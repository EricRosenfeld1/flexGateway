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

namespace flexGateway.Server
{
    public static class HostExtension
    {
        public static IHost LoadPlugins(this IHost host)
        {
            var am = host.Services.GetService<IAdapterFactory>();
            var nm = host.Services.GetService<INodeFactory>();

            var pluginManager = new PluginManager(am, nm);
            pluginManager.LoadPlugins();

            return host;
        }
    }
}
