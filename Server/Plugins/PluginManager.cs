using flexGateway.Common.Adapters;
using flexGateway.Common.Nodes;
using flexGateway.Plugin;
using flexGateway.Plugin.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;

namespace flexGateway.Common.Plugins
{
    public class PluginManager
    {
        private IAdapterFactory deviceFactory;
        private INodeFactory nodeFactory;
        public PluginManager(IAdapterFactory deviceFactory, INodeFactory nodeFactory)
        {
            this.deviceFactory = deviceFactory;
            this.nodeFactory = nodeFactory;
        }

        public void LoadPlugins(ILogger<PluginManager> logger)
        {
            string pluginPath = Path.Combine(AppContext.BaseDirectory, "plugins");

            if (!Directory.Exists(pluginPath))
            {
                logger.LogError("Plugin path not found!");
                return;
            }

            foreach (var dir in Directory.GetDirectories(pluginPath))
            {
                try
                {
                    string name = Path.GetFileName(dir) + ".dll";
                    Assembly plugin = LoadPlugin(Path.Combine(dir, name));

                    Type[] types = plugin.GetTypes();
                    Type adapterType = null;
                    Type nodeType = null;
                    Type configType = null;
                    Type nodeConfigType = null;

                    for (int i = 0; i <= types.Length - 1; i++)
                        if (typeof(Adapter).IsAssignableFrom(types[i]))
                        {
                            adapterType = types[i];
                        }
                        else if (typeof(Node).IsAssignableFrom(types[i]))
                        {
                            nodeType = types[i];
                        }
                        else if (typeof(IAdapterConfiguration).IsAssignableFrom(types[i]))
                        {
                            configType = types[i];
                        }
                        else if (typeof(INodeConfiguration).IsAssignableFrom(types[i]))
                        {
                            nodeConfigType = types[i];
                        }

                    if (adapterType == null || nodeType == null || configType == null || nodeConfigType == null)
                        throw new Exception("Plugin error");

                    deviceFactory.Register(adapterType, configType);
                    nodeFactory.Register(adapterType, nodeType, nodeConfigType);

                    logger.LogInformation($"Plugin: {adapterType.Name} loaded");

                }
                catch (Exception ex)
                {
                    logger.LogInformation(ex.Message);
                }
            }       
        }

        private bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }

        private static Assembly LoadPlugin(string path)
        {
            var s = PluginLoadContext.All;
            PluginLoadContext loadContext = new PluginLoadContext(path);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(path)));
        }
    }


}
