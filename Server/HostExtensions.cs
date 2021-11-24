using flexGateway.Common.Adapters;
using flexGateway.Common.Nodes;
using flexGateway.Common.Plugins;
using Microsoft.Extensions.DependencyInjection;
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
