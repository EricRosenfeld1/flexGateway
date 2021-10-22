using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace flexGateway.Interface
{
    /// <summary>
    /// Provides connectivity to a PLC, CNC, industrial device or similar
    /// </summary>
    public interface IDevice
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public bool IsSource { get; set; } 

        /// <summary>
        /// JSON configuration string which contains all data to build the device
        /// </summary>
        public IDeviceConfiguration Configuration { get; }

        /// <summary>
        /// Retreive a list of <see cref="INode"/> where the value has changed.
        /// </summary>
        /// <returns></returns>
        public Task<List<INode>> GetDirtyNodesAsync();

        /// <summary>
        /// Update nodes with new values
        /// </summary>
        /// <param name="changes"></param>
        /// <returns></returns>
        public Task PushChangesAsync(Dictionary<INode, object> changes);

        public Task ConnectAsync();

        public Task DisconnectAsync();

        public void AddNode(INode node);

        public List<INode> GetNodes();
    }
}
