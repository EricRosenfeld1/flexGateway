using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using flexGateway.Interface;
using Newtonsoft.Json;

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

            Type configType;
            bool success = RegisteredTypes.TryGetValue(adapterType, out configType);

            if (success)
            {
                var config = JsonConvert.DeserializeObject(configAsJson, configType);

                object[] paras = { name, guid, config };
                var instance = Activator.CreateInstance(adapterType, paras);
                return (IAdapter)instance;
            } else
                return null;
        }

        public T Create<T>(string name, Guid guid, string configAsJson) where T : IAdapter
        {
            return (T)Create(typeof(T), name, guid, configAsJson);
        }
    }
}
