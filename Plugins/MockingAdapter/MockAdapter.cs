using flexGateway.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MockingAdapter
{
    public class MockAdapter : IAdapter
    {
        private HashSet<MockNode> _nodes { get; set; }

        public string Name { get; set; }
        public Guid Guid { get; set; }
        public IAdapterConfiguration Configuration { get; set; }
        public MockAdapter(string name, Guid guid, MockConfiguration config)
        {
            Name = name;
            Guid = guid;
            Configuration = config;
        }

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

        public void AddNode(INode node)
        {
            throw new NotImplementedException();
        }
    }
}
