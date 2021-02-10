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
            var af = host.Services.GetService<IAdapterFactory>();
            var nf = host.Services.GetService<INodeFactory>();

            var pluginManager = new PluginManager(af, nf);
            pluginManager.LoadPlugins();

            return host;
        }
    }
}
