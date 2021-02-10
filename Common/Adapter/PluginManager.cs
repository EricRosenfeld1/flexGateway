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
                string name = Path.GetFileName(dir) + ".dll";
                Assembly plugin = LoadPlugin(Path.Combine(dir, name));

                Type[] types = plugin.GetTypes();
                for(int i=0;i<=types.Length -1; i++)
                {
                    if (typeof(IAdapter).IsAssignableFrom(types[i]))
                    {                      
                        _adapterManager.Register(types[i]);
                    } else if (typeof(INode).IsAssignableFrom(types[i]))
                    {
                        _nodeFactory.Register(types[i]);
                    }
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
