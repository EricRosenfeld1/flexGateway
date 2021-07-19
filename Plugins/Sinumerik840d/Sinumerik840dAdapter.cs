using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetSiemensPLCToolBoxLibrary.Communication;
using flexGateway.Interface;
using Newtonsoft.Json;

namespace Sinumerik840d
{
    public class Sinumerik840dAdapter : IAdapter
    {
        private PLCConnection _connection { get; set; }
        private PLCConnectionConfiguration _plcConfig { get; set; }
        private HashSet<Sinumerik840dNode> _nodes { get; set; } = new();

        public string Name { get; set; }
        public Guid Guid { get; private set; }
        public IAdapterConfiguration Configuration { get; private set; }

        public Sinumerik840dAdapter(string name, Guid guid, Siumerik840dConfiguration config)
        {
            Name = name;
            Guid = guid;
            Configuration = config;

            var plcConfig = new PLCConnectionConfiguration()
            {
                ConnectionName = name,
                ConnectionType = LibNodaveConnectionTypes.ISO_over_TCP,
                CpuIP = config.IpAdress,
                Port = 22,
                CpuRack = config.Rack,
                CpuSlot = config.Slot,
                Timeout = TimeSpan.FromSeconds(5)
            };

            _connection = new PLCConnection(plcConfig);
        }

        public Task ConnectAsync()
        {
            _connection.Connect();
            return Task.CompletedTask;
        }

        public Task DisconnectAsync()
        {
            _connection.Disconnect();
            return Task.CompletedTask;
        }

        public Task<List<INode>> GetDirtyNodesAsync()
        {
            List<INode> dirtyNodes = new();
            foreach (var node in _nodes)
            {
                // TODO: we can pass a list for perfomance reasons, the library does optimization
                var value = _connection.ReadValue(node.NCVar);
                if (value != node.Value)
                    dirtyNodes.Add(node);
            }
            return Task.FromResult(dirtyNodes);
        }

        public Task PushChangesAsync(Dictionary<INode, object> changes)
        {
            Dictionary<INode, Sinumerik840dNode> bindings = _nodes.ToDictionary(x => x.ParentNode, x => x);

            foreach (var sourceChange in changes.Keys)
            {
                var sinumerikNode = bindings[sourceChange];
                PLCNckTag tag = new PLCNckTag(sinumerikNode.NCVar) { Value = changes[sourceChange] };

                // TODO: we can pass a list for perfomance reasons, the library does optimization
                _connection.WriteValue(tag);
                bindings[sourceChange].Value = changes[sourceChange];
            }
            return Task.CompletedTask;
        }

        public void AddNode(INode node) 
        {
            _nodes.Add(node as Sinumerik840dNode);
        }
    }

}
