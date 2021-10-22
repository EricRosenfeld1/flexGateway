using System;


namespace flexGateway.Interface
{
    public interface INode
    {
        public Guid Guid { get; set; }
        public string NodeName { get; set; }
        public Guid ParentGuid { get; set; }
        public object Value { get; set; }
        public NodeDataType NodeType { get; set; }
        public INodeConfiguration Configuration { get; }
    }
    public enum NodeDataType
    {
        String = 0,
        Int = 1,
        Double = 2
    }
}
