using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using flexGateway.Interface;

namespace flexGateway.Common.Adapter
{
    public class AdapterFactory : IAdapterFactory
    {
        public Dictionary<Type, Type> RegisteredTypes { get; private set; } = new();
        public void Register(Type adapterType, Type configType)
        {
            if(typeof(IAdapter).IsAssignableFrom(adapterType))
                if(typeof(IAdapterConfiguration).IsAssignableFrom(configType))
                    RegisteredTypes.Add(adapterType, configType);
        }
        public IAdapter Create(Type adapterType, string name, Guid guid, string configAsJson)
        {
            if (!RegisteredTypes.Keys.Contains(adapterType))
                throw new Exception("Type not registered");

            object[] paras = { name, guid, configAsJson };
            var instance = Activator.CreateInstance(adapterType, paras);
            return (IAdapter)instance;
        }

        public T Create<T>(string name, Guid guid, string configAsJson) where T : IAdapter
        {
            return (T)Create(typeof(T), name, guid, configAsJson);
        }
    }
}
