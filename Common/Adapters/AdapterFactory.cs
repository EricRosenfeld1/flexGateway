using flexGateway.Plugin;
using flexGateway.Plugin.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace flexGateway.Common.Adapters
{
    public class AdapterFactory : IAdapterFactory
    {
        public Dictionary<Type, Type> RegisteredTypes { get; private set; } = new();

        public void Register(Type adapterType, Type configType)
        {
            if (typeof(Adapter).IsAssignableFrom(adapterType))
                if (typeof(IAdapterConfiguration).IsAssignableFrom(configType))
                    RegisteredTypes.Add(adapterType, configType);
        }

        public Adapter Create(string typeFullName, string configAsJson)
        {
            Type adapterType = RegisteredTypes.Keys.Where(x => x.FullName == typeFullName).First();
            if (adapterType == null)
                throw new Exception("Type not registered");

            Type configType;
            bool success = RegisteredTypes.TryGetValue(adapterType, out configType);

            if (success)
            {
                var config = JsonConvert.DeserializeObject(configAsJson, configType);

                var instance = Activator.CreateInstance(adapterType);
                var adapter = (Adapter)instance;

                adapter.Configure((IAdapterConfiguration)config);

                return (Adapter)instance;
            }
            else
                throw new Exception("Type configuration not found");
        }
    }
}
