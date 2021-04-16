using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using flexGateway.Common.AdapterNode;
using flexGateway.Interface;
using Microsoft.Extensions.Logging;

namespace flexGateway.Common.Adapter
{
    public class PluginManager
    {
        private IAdapterFactory _adapterFactory;
        private INodeFactory _nodeFactory;
        public PluginManager(IAdapterFactory adapterFactory, INodeFactory nodeFactory)
        {
            _adapterFactory = adapterFactory;
            _nodeFactory = nodeFactory;
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
                    Type adapterType = null;
                    Type nodeType = null;

                    for (int i = 0; i <= types.Length - 1; i++)
                    {
                        if (typeof(IAdapter).IsAssignableFrom(types[i]))
                        {
                            adapterType = types[i];
                        }
                        else if (typeof(INode).IsAssignableFrom(types[i]))
                        {
                            nodeType = types[i];
                        }
                    }

                    if (adapterType == null || nodeType == null)
                        throw new Exception("Plugin error");

                    _adapterFactory.Register(adapterType);
                    _nodeFactory.Register(adapterType, nodeType);

                    logger.LogInformation($"Plugin: {adapterType.Name} loaded");

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
