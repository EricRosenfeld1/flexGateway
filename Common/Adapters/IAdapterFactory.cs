using flexGateway.Plugin;
using System;
using System.Collections.Generic;

namespace flexGateway.Common.Adapters
{
    public interface IAdapterFactory
    {
        /// <summary>
        /// All registered adapter types including their configuration type
        /// </summary>
        Dictionary<Type, Type> RegisteredTypes { get; }
        Adapter Create(string typeFullName, string configAsJson);
        void Register(Type adapterType, Type configType);
    }
}