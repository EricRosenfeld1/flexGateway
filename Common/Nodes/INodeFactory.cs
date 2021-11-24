using flexGateway.Plugin;
using System;
using System.Collections.Generic;

namespace flexGateway.Common.Nodes
{
    public interface INodeFactory
    {
        Dictionary<Type, Type> RegisteredTypes { get; }
        Dictionary<Type, Type> ConfigurationTypes { get; }

        void Register(Type adapterType, Type nodeType, Type configType);
        Node Create(string typeFullName, string configAsJson);
    }
}