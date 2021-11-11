using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace flexGateway.Interface
{
    /// <summary>
    /// Provides connectivity to a device
    /// </summary>
    public interface IDevice
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public bool IsSource { get; set; } 
        public bool IsConnected { get; set; }

        /// <summary>
        /// JSON formatted configuration
        /// </summary>
        public IDeviceConfiguration Configuration { get; }

        public Exception LastException { get; set; }

        /// <summary>
        /// Get nodes from device where the values has changed
        /// </summary>
        public Task<List<INode>> GetDirtyNodesAsync();

        /// <summary>
        /// Update nodes with new values
        /// </summary>
        /// <param name="changes">Contains node guid and its value</param>
        public Task PushChangesAsync(Dictionary<Guid, object> changes);

        /// <summary>
        /// Update nodes from parent with new values; if parent guid is <see cref="Guid.Empty>"/> the value has no assigned parent
        /// </summary>
        /// <param name="changes">Contains parent node guid and its value</param>
        public Task PushParentChangesAsync(Dictionary<Guid, object> changes);

        public Task ConnectAsync();

        public Task DisconnectAsync();

        public void AddNode(INode node);

        public List<INode> GetNodes();
    }
}
