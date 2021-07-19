using flexGateway.Interface;
using System;
using System.Collections.Generic;

namespace flexGateway.Common.AdapterNode
{
    public interface INodeFactory
    {
        Dictionary<Type, Type> RegisteredTypes { get; }
        Dictionary<Type, Type> ConfigurationTypes { get; }
        void Register(Type adapterType, Type nodeType, Type configType);
        INode Create(Type nodeType, string name, Guid guid, string configAsJson);
    }
}