using flexGateway.Plugin;
using flexGateway.Plugin.Configuration;
using flexGateway.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace flexGateway.Common.Nodes
{
    public class NodeFactory : INodeFactory
    {
        public Dictionary<Type, Type> RegisteredTypes { get; private set; } = new();
        public Dictionary<Type, Type> ConfigurationTypes { get; private set; } = new();

        public void Register(Type adapterType, Type nodeType, Type configType)
        {
            if (typeof(Adapter).IsAssignableFrom(adapterType))
                if (typeof(Adapter).IsAssignableFrom(nodeType))
                    if (typeof(IAdapterConfiguration).IsAssignableFrom(configType))
                    {
                        RegisteredTypes.Add(adapterType, nodeType);
                        ConfigurationTypes.Add(nodeType, configType);
                    }
        }
        public Node Create(string typeFullName, string configAsJson)
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
                return (Node)instance;
            }
            else
                return null;
        }

        public Node Create(NodeModel nodeModel)
        {
            var node = Create(nodeModel.TypeFullName, nodeModel.JsonConfiguration);
            node.Guid = nodeModel.Guid;
            node.Name = nodeModel.Name;
            node.ParentGuid = nodeModel.ParentGuid;
            node.DataType = nodeModel.DataType;
            
            return node;
        }
    }
}
