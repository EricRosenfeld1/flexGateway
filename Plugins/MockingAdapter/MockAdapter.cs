using flexGateway.Plugin;
using flexGateway.Plugin.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockingAdapter
{
    public class MockAdapter : Adapter
    {
        private HashSet<MockNode> _nodes { get; set; } = new();
        private int _seed;

        public override void Configure(IAdapterConfiguration configuration)
        {
            var c = configuration as MockConfiguration;
            _seed = c.Seed;
            base.Configure(configuration);
        }

        public override Task ConnectAsync()
        {
            return Task.CompletedTask;
        }

        public override Task DisconnectAsync()
        {
            return Task.CompletedTask;
        }

        public override Task<List<Node>> GetDirtyNodesAsync()
        {
            List<Node> dirtyNodes = new();
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

        public override Task PushParentChangesAsync(HashSet<NodeChange> parentChanges)
        {
            Dictionary<Guid, MockNode> bindings = _nodes.ToDictionary(x => x.ParentGuid, x => x);
            foreach (var parentChange in parentChanges)
                bindings[parentChange.Guid].Value = parentChange.Value;

            return Task.CompletedTask;
        }

        public override Task PushChangesAsync(HashSet<NodeChange> changes)
        {
            Dictionary<Guid, MockNode> bindings = _nodes.ToDictionary(x => x.Guid, x => x);
            foreach (var change in changes)
                bindings[change.Guid].Value = change.Value;

            return Task.CompletedTask;
        }

        public override void AddNode(Node node)
        {
            _nodes.Add(node as MockNode);
        }

        public override List<Node> GetNodes()
        {
            return _nodes.ToList<Node>();
        }

    }
}
