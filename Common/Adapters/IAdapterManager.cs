using flexGateway.Plugin;
using flexGateway.Shared;
using System;
using System.Collections.Generic;

namespace flexGateway.Common.Adapters
{
    public interface IAdapterManager
    {
        public List<Adapter> Adapters { get; }
        public bool AddAdapter(Adapter adapter);
        public Guid AddAdapter(AdapterConfigurationModel adapter);
        public bool RemoveAdapter(Guid adapterGuid);
    }
}
