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
        /// List of <see cref="INode"/> implemented by this <see cref="IAdapter"/>.
        /// Nodes should only be added/removed through <see cref="RemoveNode(Guid)"/> and <see cref="AddNode(INode)"/> due to duplicate checking.
        /// </summary>
        public List<INode> Nodes { get; }

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

        /// <summary>
        /// Add a <see cref="T"/> to the <see cref="Nodes"/>
        /// </summary>
        /// <param name="node"></param> 
        /// <exception cref="NotImplementedException">Throw if node is wrong type</exception>
        public void AddNode(INode node);

        public void RemoveNode(Guid nodeGuid);

    }
}
