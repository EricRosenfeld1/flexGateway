using flexGateway.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MockingAdapter
{
    public class MockAdapter : IAdapter
    {
        public string Name { get; set; }

        public Guid Guid { get; set; }

        private HashSet<MockNode> _nodes { get; set; }

        public void AddNode(string jsonConfig)
        {
            throw new NotImplementedException();
        }

        public Task ConnectAsync()
        {
            throw new NotImplementedException();
        }

        public Task DisconnectAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<INode>> GetDirtyNodesAsync()
        {
            throw new NotImplementedException();
        }

        public Task PushChangesAsync(Dictionary<INode, object> changes)
        {
            throw new NotImplementedException();
        }
    }
}
