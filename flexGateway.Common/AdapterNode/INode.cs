using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flexGateway.Common.MachineNode
{
    public interface INode
    {
        public Guid Guid { get; }
        public string NodeName { get; set; }
        public INode ParentNode { get; set; }
        public object Value { get; set; }
        public NodeDataType NodeType { get; set; }
    }

    public enum NodeDataType
    {
        String = 0,
        Int = 1,
        Double = 2
    }
}
