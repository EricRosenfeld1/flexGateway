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
        /// Retreive a list of <see cref="INode"/> where the value has changed. This should not change the actual <see cref="Nodes"/> state.
        /// </summary>
        /// <returns></returns>
        public Task<List<INode>> GetDirtyNodesAsync();

        /// <summary>
        /// Merge the changed <see cref="INode"/> values from parent to the <see cref="Nodes"/>.
        /// </summary>
        /// <param name="changes"></param>
        /// <returns></returns>
        public Task PushChangesAsync(Dictionary<INode, object> changes);

        public Task ConnectAsync();
        public Task DisconnectAsync();

        public void AddNode(string jsonConfig);
    }
}
