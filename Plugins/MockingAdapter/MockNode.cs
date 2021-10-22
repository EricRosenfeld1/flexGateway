using flexGateway.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MockingAdapter
{
    public class MockNode : INode
    {
        public Guid Guid { get; set; }
        public string NodeName { get; set; }
        public Guid ParentGuid { get; set; }
        public object Value { get; set; }
        public NodeDataType NodeType { get; set; }
        public INodeConfiguration Configuration { get; set; }

        public int Max { get; set; }
        public int Min { get; set; }
        public MockNode(MockNodeConfiguration config)
        {
            Configuration = config;
            Max = config.Max;
            Min = config.Min;
        }
    }
}
