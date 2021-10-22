using flexGateway.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace flexGateway.Common.Node
{
    public class NodeFactory : INodeFactory
    {
        public Dictionary<Type, Type> RegisteredTypes { get; private set; } = new();
        public Dictionary<Type, Type> ConfigurationTypes { get; private set; } = new();
        public void Register(Type deviceType, Type nodeType, Type configType)
        {
            if (typeof(IDevice).IsAssignableFrom(deviceType))
                if (typeof(INode).IsAssignableFrom(nodeType))
                    if (typeof(INodeConfiguration).IsAssignableFrom(configType))
                    {
                        RegisteredTypes.Add(deviceType, nodeType);
                        ConfigurationTypes.Add(nodeType, configType);
                    }
        }

        public INode Create(string typeFullName, string configAsJson)
        {
            Type nodeType = ConfigurationTypes.Where(x => x.Value.FullName == typeFullName).FirstOrDefault().Key;
            if (nodeType == null)
                throw new Exception("Type not registered");

            Type configType;
            bool success = ConfigurationTypes.TryGetValue(nodeType, out configType);

            if (success)
            {
                var config = JsonConvert.DeserializeObject(configAsJson, configType);

                object[] paras = { config };
                var instance = Activator.CreateInstance(nodeType, paras);
                return (INode)instance;
            }
            else
                return null;
        }
    }
}
