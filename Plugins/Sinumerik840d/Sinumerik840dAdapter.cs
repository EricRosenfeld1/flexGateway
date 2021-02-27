using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetSiemensPLCToolBoxLibrary.Communication;
using flexGateway.Interface;

namespace Sinumerik840d
{
    public class Sinumerik840dAdapter : IAdapter
    {
        private PLCConnection _connection { get; set; }
        private PLCConnectionConfiguration _plcConfig { get; set; }
        private HashSet<Sinumerik840dNode> _nodes { get; set; } = new();

        public string Name { get; set; }
        public Guid Guid { get; private set; }
        public string Configuration { get; private set; }

        public Sinumerik840dAdapter(string name, Guid guid, string configuration)
        {
            Name = name;
            Guid = guid;
            Configuration = configuration;

            _plcConfig = SinumerikConfigurationParser.ParseConfig(configuration);
            _connection = new PLCConnection(_plcConfig);
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
                var sNode = node;
                var value = _connection.ReadValue(sNode.ToNC_Var());
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
                PLCNckTag tag = new PLCNckTag(sinumerikNode.ToNC_Var()) { Value = changes[sourceChange] };

                // TODO: we can pass a list for perfomance reasons, the library does optimization
                _connection.WriteValue(tag);
                bindings[sourceChange].Value = changes[sourceChange];
            }
            return Task.CompletedTask;
        }

        public void AddNode(INode node) 
        {

        }
    }

    static class SinumerikConfigurationParser
    {
        public static PLCConnectionConfiguration ParseConfig(string json)
        {
            var config = new PLCConnectionConfiguration("SinumerikConnection");      
            return config;
        }
    }

    static class SinumerikNodeParser
    {
        public static Sinumerik840dNode ParseNode(string json)
        {
            return new Sinumerik840dNode(new Guid(), "") ;
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
