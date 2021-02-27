using flexGateway.Interface;
using System;
using System.Collections.Generic;

namespace flexGateway.Common.AdapterNode
{
    public interface INodeFactory
    {
        Dictionary<Type, Type> RegisteredNodeTypes { get; }
        void Register(Type adapterType, Type nodeType);
    }
}