using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flexGateway.Interface;

namespace flexGateway.Common.Adapter
{
    public class AdapterFactory : IAdapterFactory
    {
        public HashSet<Type> RegisteredTypes { get; private set; } = new();
        public void Register(Type type)
        {
            if(typeof(IAdapter).IsAssignableFrom(type))
                RegisteredTypes.Add(type);
        }

        public IAdapter Create(Type type, string name, Guid guid, string configuration)
        {
            if (!RegisteredTypes.Contains(type))
                throw new Exception("Type not registered");

            object[] paras = { name, guid, configuration };
            var instance = Activator.CreateInstance(type, paras);
            return (IAdapter)instance;
        }

        public T Create<T>(string name, Guid guid, string config) where T : IAdapter
        {
            return (T)Create(typeof(T), name, guid, config);
        }
    }
}
