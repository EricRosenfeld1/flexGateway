using flexGateway.Interface;
using System;
using System.Collections.Generic;

namespace flexGateway.Common.Adapter
{
    public interface IAdapterFactory
    {
        /// <summary>
        /// All registered adapter types including their configuration type
        /// </summary>
        Dictionary<Type, Type> RegisteredTypes { get; }
        IAdapter Create(Type type, string name, Guid guid, string config);
        T Create<T>(string name, Guid guid, string config) where T : IAdapter;
        void Register(Type adapterType, Type configType);
    }
}