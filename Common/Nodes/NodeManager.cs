using flexGateway.Plugin;
using flexGateway.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flexGateway.Common.Nodes
{
    public class NodeManager
    {
        private readonly INodeFactory _nodeFactory;

        private Dictionary<Guid, List<Node>> _nodes;
  
        public NodeManager(INodeFactory nodeFactory)
        {
            _nodeFactory = nodeFactory;
        }

        public List<Node> GetNodes(Guid adapterGuid)
        {
            var r = new List<Node>();
            _nodes.TryGetValue(adapterGuid,out r);
            return r;
        }

        public Node AddNode(NodeConfigurationModel configurationModel)
        {
            var node = _nodeFactory.Create(configurationModel.TypeFullName, configurationModel.JsonConfiguration);
            node.Name = configurationModel.Name;
            node.ParentGuid = configurationModel.ParentGuid;

            if (_nodes.ContainsKey(configurationModel.DeviceGuid))
            {
                var l = _nodes[configurationModel.DeviceGuid];
                l.Add(node);
                return node;
            }
            else
            {
                _nodes.Add(configurationModel.DeviceGuid, new List<Node>() { node });
                return node;
            }            
        }
    }
}
