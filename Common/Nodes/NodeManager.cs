using flexGateway.Plugin;
using flexGateway.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flexGateway.Common.Nodes
{
    public class NodeManager : INodeManager
    {
        private readonly INodeFactory _nodeFactory;
        private readonly ILogger<NodeManager> _logger;

        // AdapterGuid, List<Node>
        private Dictionary<Guid, List<Node>> _nodes = new Dictionary<Guid, List<Node>>();
        private Dictionary<Guid, List<Node>> _bindings = new Dictionary<Guid, List<Node>>();

        public NodeManager(ILogger<NodeManager> logger, INodeFactory nodeFactory)
        {
            _logger = logger;
            _nodeFactory = nodeFactory;
        }

        public List<Node> GetNodes(Guid adapterGuid)
        {
            var nodes = new List<Node>();
            _nodes.TryGetValue(adapterGuid, out nodes);
            return nodes;
        }

        public Dictionary<Guid, List<Node>> GetParentBindings()
        {
            return _bindings;
        }

        public Node AddNode(NodeConfigurationModel configurationModel)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public bool RemoveNode(Guid nodeGuid)
        {
            foreach (var nodes in _nodes.Values)
                foreach (var node in nodes)
                {
                    var index = nodes.FindIndex(x => x.Guid == nodeGuid);
                    if (index >= 0)
                    {
                        nodes.RemoveAt(index);
                        return true;
                    }
                }
            return false;
        }
    }
}
