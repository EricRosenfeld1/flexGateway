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

namespace flexGateway.Common.Adapter
{
    public class PluginManager
    {
        private IAdapterFactory _adapterManager;
        private INodeFactory _nodeFactory;
        public PluginManager(IAdapterFactory adapterFactory, INodeFactory nodeFactory)
        {
            _adapterManager = adapterFactory;
            _nodeFactory = nodeFactory;
        }
        public void LoadPlugins()
        {
            string pluginPath = AppContext.BaseDirectory + "plugins\\";
            foreach (var dir in Directory.GetDirectories(pluginPath))
            {
                string name = Path.GetDirectoryName(dir) + ".dll";
                Assembly plugin = LoadPlugin(Path.Combine(dir, name));

                foreach (Type type in plugin.GetTypes())
                {
                    if (typeof(IAdapter).IsAssignableFrom(type))
                    {                      
                        _adapterManager.Register(type);
                    } else if (typeof(INode).IsAssignableFrom(type))
                    {
                        _nodeFactory.Register(type);
                    }
                }
            }
        }

        private static Assembly LoadPlugin(string path)
        {
            PluginLoadContext loadContext = new PluginLoadContext(path);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(path)));
        }
    }


}
