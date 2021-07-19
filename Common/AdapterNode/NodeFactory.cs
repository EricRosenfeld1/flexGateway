using flexGateway.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace flexGateway.Common.AdapterNode
{
    public class NodeFactory : INodeFactory
    {
        public Dictionary<Type, Type> RegisteredTypes { get; private set; } = new();
        public Dictionary<Type, Type> ConfigurationTypes { get; private set; } = new();
        public void Register(Type adapterType, Type nodeType, Type configType)
        {
            if (typeof(IAdapter).IsAssignableFrom(adapterType))
                if (typeof(INode).IsAssignableFrom(nodeType))
                    if (typeof(INodeConfiguration).IsAssignableFrom(configType))
                    {
                        RegisteredTypes.Add(adapterType, nodeType);
                        ConfigurationTypes.Add(nodeType, configType);
                    }
        }

        public INode Create(Type nodeType, string name, Guid guid, string configAsJson)
        {
            if (!RegisteredTypes.Keys.Contains(nodeType))
                throw new Exception("Type not registered");

            Type configType;
            bool success = RegisteredTypes.TryGetValue(nodeType, out configType);

            if (success)
            {
                var config = JsonConvert.DeserializeObject(configAsJson, configType);

                object[] paras = { name, guid, config };
                var instance = Activator.CreateInstance(nodeType, paras);
                return (INode)instance;
            }
            else
                return null;
        }
    }
}
