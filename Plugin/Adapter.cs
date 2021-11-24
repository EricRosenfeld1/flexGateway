using flexGateway.Plugin.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace flexGateway.Plugin
{
    public abstract class Adapter
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public bool IsSource { get; set; }
        public bool IsConnected { get; set; }
        public IAdapterConfiguration Configuration { get; set; }
        public Exception LastException { get; set; }

        public virtual void Configure(IAdapterConfiguration configuration)
        {
            Configuration = configuration;
        }

        public abstract void AddNode(Node node);

        public abstract Task ConnectAsync();

        public abstract Task DisconnectAsync();

        public abstract Task<List<Node>> GetDirtyNodesAsync();

        public abstract List<Node> GetNodes();

        public abstract Task PushChangesAsync(HashSet<NodeChange> changes);

        public abstract Task PushParentChangesAsync(HashSet<NodeChange> parentChanges);
    }
}
