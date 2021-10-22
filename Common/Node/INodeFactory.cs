using flexGateway.Interface;
using System;
using System.Collections.Generic;

namespace flexGateway.Common.Node
{
    public interface INodeFactory
    {
        Dictionary<Type, Type> RegisteredTypes { get; }
        Dictionary<Type, Type> ConfigurationTypes { get; }
        void Register(Type deviceType, Type nodeType, Type configType);
        INode Create(string typeFullName, string configAsJson);
    }
}