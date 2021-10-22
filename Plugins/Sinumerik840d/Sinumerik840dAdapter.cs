using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetSiemensPLCToolBoxLibrary.Communication;
using flexGateway.Interface;
using Newtonsoft.Json;

namespace Sinumerik840d
{
    public class Sinumerik840dAdapter : IDevice
    {
        private PLCConnection connection { get; set; }
        private HashSet<Sinumerik840dNode> nodes { get; set; } = new();

        public string Name { get; set; }
        public Guid Guid { get; set; }
        public IDeviceConfiguration Configuration { get; private set; }
        public bool IsSource { get; set; }
        public bool IsConnected { get; set; }
        public Exception LastException { get; set; }

        public Sinumerik840dAdapter(Siumerik840dConfiguration config)
        {
            Configuration = config;

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

            connection = new PLCConnection(plcConfig);
        }

        public Task ConnectAsync()
        {
            connection.Connect();
            return Task.CompletedTask;
        }

        public Task DisconnectAsync()
        {
            connection.Disconnect();
            return Task.CompletedTask;
        }

        public Task<List<INode>> GetDirtyNodesAsync()
        {
            List<INode> dirtyNodes = new();
            foreach (var node in nodes)
            {
                // TODO: we can pass a list for perfomance reasons, the library does optimization
                var value = connection.ReadValue(node.NCVar);
                if (value != node.Value)
                    dirtyNodes.Add(node);
            }
            return Task.FromResult(dirtyNodes);
        }

        public Task PushChangesAsync(Dictionary<Guid, object> changes)
        {
            Dictionary<Guid, Sinumerik840dNode> bindings = nodes.ToDictionary(x => x.Guid, x => x);

            foreach (var sourceChange in changes.Keys)
            {
                var sinumerikNode = bindings[sourceChange];
                PLCNckTag tag = new PLCNckTag(sinumerikNode.NCVar) { Value = changes[sourceChange] };

                // TODO: we can pass a list for perfomance reasons, the library does optimization
                connection.WriteValue(tag);
                bindings[sourceChange].Value = changes[sourceChange];
            }

            return Task.CompletedTask;
        }

        public Task PushParentChangesAsync(Dictionary<Guid, object> changes)
        {
            Dictionary<Guid, Sinumerik840dNode> bindings = nodes.ToDictionary(x => x.ParentGuid, x => x);

            foreach (var sourceChange in changes.Keys)
            {
                var sinumerikNode = bindings[sourceChange];
                PLCNckTag tag = new PLCNckTag(sinumerikNode.NCVar) { Value = changes[sourceChange] };

                // TODO: we can pass a list for perfomance reasons, the library does optimization
                connection.WriteValue(tag);
                bindings[sourceChange].Value = changes[sourceChange];
            }

            return Task.CompletedTask;
        }

        public void AddNode(INode node) 
        {
            nodes.Add(node as Sinumerik840dNode);
        }

        public List<INode> GetNodes()
        {
            return nodes.ToList<INode>();
        }
    }

}
