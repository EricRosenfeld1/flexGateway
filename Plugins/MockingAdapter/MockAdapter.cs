using flexGateway.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MockingAdapter
{
    public class MockAdapter : IDevice
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public bool IsSource { get; set; }
        public IDeviceConfiguration Configuration { get; set; }
        public Exception LastException { get; set; }
        public bool IsConnected { get; set; }

        private HashSet<MockNode> _nodes { get; set; } = new();
        private int _seed;

        public MockAdapter(MockConfiguration config)
        {
            Configuration = config;
            _seed = config.Seed;
        }

        public Task ConnectAsync()
        {
            return Task.CompletedTask;
        }

        public Task DisconnectAsync()
        {
            return Task.CompletedTask;
        }

        public Task<List<INode>> GetDirtyNodesAsync()

        {
            List<INode> dirtyNodes = new();
            foreach (var node in _nodes)
            {
                var rnd = new Random(_seed);
                var val = rnd.Next(node.Min, node.Min);

                if ((int)node.Value != val)
                {
                    node.Value = val;
                    dirtyNodes.Add(node);
                }
            }

            return Task.FromResult(dirtyNodes);
        }

        public Task PushParentChangesAsync(Dictionary<Guid, object> changes)
        {
            Dictionary<Guid, MockNode> bindings = _nodes.ToDictionary(x => x.ParentGuid, x => x);
            foreach (var parentChange in changes)
                if (parentChange.Key != Guid.Empty)
                    bindings[parentChange.Key].Value = parentChange.Value;

            return Task.CompletedTask;           
        }

        public Task PushChangesAsync(Dictionary<Guid, object> changes)
        {
            foreach (var change in changes)
                _nodes.FirstOrDefault(x => x.Guid == change.Key).Value = change.Value;

            return Task.CompletedTask;
        }

        public void AddNode(INode node)
        {
            _nodes.Add(node as MockNode);
        }

        public List<INode> GetNodes()
        {
            return _nodes.ToList<INode>();
        }


    }
}
