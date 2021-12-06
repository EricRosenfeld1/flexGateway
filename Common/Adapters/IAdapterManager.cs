using flexGateway.Plugin;
using flexGateway.Shared;
using System;
using System.Collections.Generic;

namespace flexGateway.Common.Adapters
{
    public interface IAdapterManager
    {
        public List<Adapter> Adapters { get; }
        public Adapter AddAdapter(Adapter adapter);
        public Adapter AddAdapter(AdapterConfigurationModel adapter);
        public bool RemoveAdapter(Guid adapterGuid);
    }
}
