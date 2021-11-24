using flexGateway.Plugin;
using System;

namespace flexGateway.Shared
{
    public class NodeModel
    {
        public Guid Guid { get; set; }
        public Guid ParentGuid { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public NodeDataType NodeType { get; set; }
    }

}
