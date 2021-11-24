using DotNetSiemensPLCToolBoxLibrary.Communication;
using flexGateway.Plugin;
using flexGateway.Plugin.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sinumerik840d
{
    public class Sinumerik840dAdapter : Adapter
    {
        private PLCConnection _connection { get; set; }
        private HashSet<Sinumerik840dNode> _nodes { get; set; } = new();

        public override void Configure(IAdapterConfiguration configuration)
        {
            var config = configuration as Siumerik840dConfiguration;
            var plcConfig = new PLCConnectionConfiguration()
            {
                ConnectionName = "flexGateway Connection",
                ConnectionType = LibNodaveConnectionTypes.ISO_over_TCP,
                CpuIP = config.IpAdress,
                Port = 22,
                CpuRack = config.Rack,
                CpuSlot = config.Slot,
                Timeout = TimeSpan.FromSeconds(5)
            };

            _connection = new PLCConnection(plcConfig);

            base.Configure(configuration);
        }

        public override Task ConnectAsync()
        {
            _connection.Connect();
            return Task.CompletedTask;
        }

        public override Task DisconnectAsync()
        {
            _connection.Disconnect();
            return Task.CompletedTask;
        }

        public override Task<List<Node>> GetDirtyNodesAsync()
        {
            List<Node> dirtyNodes = new();
            foreach (var node in _nodes)
            {
                // TODO: we can pass a list for perfomance reasons, the library does optimization
                var value = _connection.ReadValue(node.NCVar);
                if (value != node.Value)
                    dirtyNodes.Add(node);
            }
            return Task.FromResult(dirtyNodes);
        }

        public override Task PushChangesAsync(HashSet<NodeChange> changes)
        {
            Dictionary<Guid, Sinumerik840dNode> bindings = _nodes.ToDictionary(x => x.Guid, x => x);

            foreach (var change in changes)
            {
                var sinumerikNode = bindings[change.Guid];
                PLCNckTag tag = new PLCNckTag(sinumerikNode.NCVar) { Value = change.Value };

                // TODO: we can pass a list for perfomance reasons, the library does optimization
                _connection.WriteValue(tag);
                bindings[change.Guid].Value = change.Value;
            }

            return Task.CompletedTask;
        }

        public override Task PushParentChangesAsync(HashSet<NodeChange> changes)
        {
            Dictionary<Guid, Sinumerik840dNode> bindings = _nodes.ToDictionary(x => x.ParentGuid, x => x);

            foreach (var change in changes)
            {
                var sinumerikNode = bindings[change.Guid];
                PLCNckTag tag = new PLCNckTag(sinumerikNode.NCVar) { Value = change.Value };

                // TODO: we can pass a list for perfomance reasons, the library does optimization
                _connection.WriteValue(tag);
                bindings[change.Guid].Value = change.Value;
            }

            return Task.CompletedTask;
        }

        public override void AddNode(Node node)
        {
            _nodes.Add(node as Sinumerik840dNode);
        }

        public override List<Node> GetNodes()
        {
            return _nodes.ToList<Node>();
        }
    }

}
