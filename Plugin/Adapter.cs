using flexGateway.Plugin.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace flexGateway.Plugin
{
    /// <summary>
    /// A class which provides connectivity to a device or client used to read and write <see cref="Node">Nodes</see>
    /// </summary>
    public abstract class Adapter
    {
        /// <summary>
        /// Name of the <see cref="Adapter"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Guid of the <see cref="Adapter"/>
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the <see cref="Adapter"/> is the source
        /// </summary>
        /// <remarks>
        /// All other <see cref="Adapter">Adapters</see> will be updated based on the source <see cref="Node">Nodes</see>
        /// </remarks>
        public bool IsSource { get; set; }

        /// <summary>
        /// Get a value indicating if the adapter is connected
        /// </summary>
        public bool IsConnected { get;  set; }

        /// <summary>
        /// Gets the configuration
        /// </summary>
        /// <remarks>This will be used to build the adapter. Use the <see cref="Configure(IAdapterConfiguration)"/> function to set the configuration</remarks>
        /// 
        public IAdapterConfiguration Configuration { get;  set; }

        /// <summary>
        /// Gets or sets the last occured exception
        /// </summary>
        public Exception LastException { get; set; }

        /// <summary>
        /// Configure the <see cref="Adapter"/>
        /// </summary>
        /// <param name="configuration"></param>
        public virtual void Configure(IAdapterConfiguration configuration)
        {
            Configuration = configuration;
        }

        public abstract void AddNode(Node node);

        public abstract Task ConnectAsync();

        public abstract Task DisconnectAsync();

        /// <summary>
        /// Gets all <see cref="Node">Nodes</see> where the <see cref="Node.Value">Value</see> has changed since the last poll
        /// </summary>
        /// <returns></returns>
        public abstract Task<List<Node>> GetDirtyNodesAsync();

        public abstract List<Node> GetNodes();

        /// <summary>
        /// Updates the values of all <see cref="Node">Nodes</see>. Uses the <see cref="Node.Guid"/> to bind to the adapter nodes
        /// </summary>
        /// <param name="changes">Nodes where the value has changed</param>
        /// <returns></returns>
        public abstract Task PushChangesAsync(HashSet<NodeChange> changes);

        /// <summary>
        /// Updates the values of all <see cref="Node">Nodes</see> where the <see cref="Node">ParentNode</see> has changed. Uses the <see cref="Node.ParentGuid"/> to bind to the adpater nodes
        /// </summary>
        /// <param name="parentChanges"></param>
        /// <returns></returns>
        public abstract Task PushParentChangesAsync(HashSet<NodeChange> parentChanges);

        public abstract Task ReadNodeAsync(List<Node> nodes);

        public abstract Task WriteNodesAsync(List<Node> nodes);
    }
}
