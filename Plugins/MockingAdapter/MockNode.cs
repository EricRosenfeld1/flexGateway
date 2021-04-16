using flexGateway.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MockingAdapter
{
    public class MockNode : INode
    {
        public Guid Guid => throw new NotImplementedException();

        public string NodeName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public INode ParentNode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public object Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public NodeDataType NodeType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
