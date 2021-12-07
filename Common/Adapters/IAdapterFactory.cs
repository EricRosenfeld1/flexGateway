using flexGateway.Plugin;
using flexGateway.Shared;
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
        Adapter Create(AdapterModel adapterModel);
        void Register(Type adapterType, Type configType);
    }
}