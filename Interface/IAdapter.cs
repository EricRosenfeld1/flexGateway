using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace flexGateway.Interface
{
    /// <summary>
    /// Provides connectivity to a PLC, CNC, industrial device or similar
    /// </summary>
    public interface IAdapter
    {
        public string Name { get; set; }
        public Guid Guid { get; }

        /// <summary>
        /// JSON configuration string which contains all data to build the adapter
        /// </summary>
        public string Configuration { get; }

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

        public void AddNode(string jsonConfig);
    }
}
