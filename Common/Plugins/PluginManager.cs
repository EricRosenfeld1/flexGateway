using System;
using System.IO;
using System.Reflection;
using flexGateway.Common.Device;
using flexGateway.Common.Node;
using flexGateway.Interface;
using Microsoft.Extensions.Logging;

namespace flexGateway.Common.Plugins
{
    public class PluginManager
    {
        private IDeviceFactory deviceFactory;
        private INodeFactory nodeFactory;
        public PluginManager(IDeviceFactory deviceFactory, INodeFactory nodeFactory)
        {
            this.deviceFactory = deviceFactory;
            this.nodeFactory = nodeFactory;
        }

        public void LoadPlugins(ILogger<PluginManager> logger)
        {
            string pluginPath = Path.Combine(AppContext.BaseDirectory, "plugins");
            foreach (var dir in Directory.GetDirectories(pluginPath))
            {
                try
                {
                    string name = Path.GetFileName(dir) + ".dll";
                    Assembly plugin = LoadPlugin(Path.Combine(dir, name));

                    Type[] types = plugin.GetTypes();
                    Type deviceType = null;
                    Type nodeType = null;
                    Type configType = null;
                    Type nodeConfigType = null;

                    for (int i = 0; i <= types.Length - 1; i++)                   
                        if (typeof(IDevice).IsAssignableFrom(types[i]))
                        {
                            deviceType = types[i];
                        } else if (typeof(INode).IsAssignableFrom(types[i]))
                        {
                            nodeType = types[i];
                        } else if (typeof(IDeviceConfiguration).IsAssignableFrom(types[i]))
                        {
                            configType = types[i];
                        }else if (typeof(INodeConfiguration).IsAssignableFrom(types[i]))
                        {
                            nodeConfigType = types[i];
                        }

                    if (deviceType == null || nodeType == null || configType == null || nodeConfigType == null)
                        throw new Exception("Plugin error");

                    deviceFactory.Register(deviceType, configType);
                    nodeFactory.Register(deviceType, nodeType, nodeConfigType);

                    logger.LogInformation($"Plugin: {deviceType.Name} loaded");

                } catch (Exception ex)
                {
                    logger.LogInformation(ex.Message);
                }
            }

        }

        private static Assembly LoadPlugin(string path)
        {
            var s = PluginLoadContext.All;
            PluginLoadContext loadContext = new PluginLoadContext(path);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(path)));
        }
    }


}
