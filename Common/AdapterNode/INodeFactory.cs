using flexGateway.Interface;
using System;
using System.Collections.Generic;

namespace flexGateway.Common.AdapterNode
{
    public interface INodeFactory
    {
        HashSet<Type> RegisteredTypes { get; }

        T Create<T>();
        void Register(Type type);
    }
}