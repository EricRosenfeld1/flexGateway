using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetSiemensPLCToolBoxLibrary.Communication;
using flexGateway.Common.Adapter;
using flexGateway.Common.MachineNode;

namespace Sinumerik840d
{
    public class Sinumerik840dAdapter : IAdapter
    {
        private PLCConnection connection { get; set; }

        public List<INode> Nodes { get; private set; }
        public string Name { get; set; }
        public PLCConnectionConfiguration Configuration { get; private set; }

        public Sinumerik840dAdapter(string name, PLCConnectionConfiguration configuration)
        {
            Name = name;
            Configuration = configuration;

            connection = new PLCConnection(configuration);
        }

        public void Connect()
        {
            connection.Connect();
        }

        public void Disconnect()
        {
            connection.Disconnect();
        }

        public Task<List<INode>> GetDirtyNodesAsync()
        {
            List<INode> dirtyNodes = new();
            foreach (var node in Nodes)
            {
                var sinumerikNode = (Sinumerik840dNode)node;
                // TODO: we can pass a list for perfomance reasons, the library does optimization
                var value = connection.ReadValue(sinumerikNode.ToNC_Var());
                if (value != node.Value)
                    dirtyNodes.Add(node);
            }
            return Task.FromResult(dirtyNodes);
        }

        public Task PushChangesAsync(Dictionary<INode, object> changes)
        {
            Dictionary<INode, INode> bindings = Nodes.ToDictionary(x => x.ParentNode, x => x);

            foreach (var sourceChange in changes.Keys)
            {
                var sinumerikNode = (Sinumerik840dNode)bindings[sourceChange];
                PLCNckTag tag = new PLCNckTag(sinumerikNode.ToNC_Var()) { Value = changes[sourceChange] };

                // TODO: we can pass a list for perfomance reasons, the library does optimization
                connection.WriteValue(tag);
                bindings[sourceChange].Value = changes[sourceChange];
            }
            return Task.CompletedTask;
        }

        public void AddNode(INode node)
        {
            if (node is Sinumerik840dNode sinumerikNode)
                if (!Nodes.Any(x => x.ParentNode == node.ParentNode || x.Guid == x.Guid))
                    Nodes.Add(sinumerikNode);
                else
                    throw new Exception("Node already exists");
            else
                throw new NotSupportedException("Invalid node type");
        }

        public void RemoveNode(Guid nodeGuid)
        {
            var node = Nodes.Find(x => x.Guid == nodeGuid);
            if (node is not null)
                Nodes.Remove(node);
        }

        public void BindNode(Guid nodeGuid, INode sourceNode)
        {
            var node = Nodes.First(x => x.Guid == sourceNode.Guid);

            if (node is not null)
                node.ParentNode = sourceNode;
        }
    }

    static class SinumerikNodeExtension
    {
        public static NC_Var ToNC_Var(this Sinumerik840dNode node)
        {
            return new NC_Var(node.Syntax, node.BereichEinheit, node.Spalte, node.Zeile, node.BausteinTyp, node.ZeilenAnzahl, node.Typ, node.Laenge);
        }
    }
}
