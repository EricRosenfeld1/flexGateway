using System;
using System.Collections.Generic;

namespace flexGateway.Plugin
{
    /// <summary>
    /// A class representing a changed value of a <see cref="Node"/>
    /// </summary>
    /// <remarks>
    /// The <see cref="Guid">Guid</see> can be the parent guid or the node guid
    /// </remarks>
    public class NodeChange
    {
        public NodeChange(Guid guid, object value, NodeDataType type)
        {
            if (guid == null)
                throw new ArgumentNullException(nameof(guid));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
        }
        public Guid Guid { get; set; }
        public object Value { get; set; }
        public NodeDataType NodeData { get; set; }
    }

    public class NodeChangeEqualityComparer : IEqualityComparer<NodeChange>
    {
        public bool Equals(NodeChange x, NodeChange y)
        {
            if (x.Guid == x.Guid)
                return true;
            else
                return false;
        }

        public int GetHashCode(NodeChange obj)
        {
            return obj.Guid.GetHashCode();
        }
    }
}
