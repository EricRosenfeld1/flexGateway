using flexGateway.Interface;
using System;
using System.Collections.Generic;

namespace flexGateway.Common.Adapter
{
    public interface IAdapterFactory
    {
        HashSet<Type> RegisteredTypes { get; }
        IAdapter Create(Type type, string name, Guid guid, string config);
        T Create<T>(string name, Guid guid, string config) where T : IAdapter;
        void Register(Type type);
    }
}