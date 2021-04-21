using flexGateway.Interface;
using System;
using System.Collections.Generic;

namespace flexGateway.Common.AdapterNode
{
    public class NodeFactory : INodeFactory
    {
        public Dictionary<Type, Type> RegisteredNodeTypes { get; private set; } = new();
        public void Register(Type adapterType, Type nodeType)
        {
            if (typeof(IAdapter).IsAssignableFrom(adapterType))
                if (typeof(INode).IsAssignableFrom(nodeType))
                    RegisteredNodeTypes.Add(adapterType, nodeType);
        }

        public INode Create(Type nodeType)
        {
            if (!RegisteredNodeTypes.ContainsValue(nodeType))
                throw new Exception("Type not registered");

            var instance = Activator.CreateInstance(nodeType);
            return (INode)instance;
        }
    }
}
