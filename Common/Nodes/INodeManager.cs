using flexGateway.Plugin;
using flexGateway.Shared;
using System;
using System.Collections.Generic;

namespace flexGateway.Common.Nodes
{
    public interface INodeManager
    {
        Node AddNode(NodeConfigurationModel configurationModel);
        List<Node> GetNodes(Guid adapterGuid);
        bool RemoveNode(Guid nodeGuid);
        Dictionary<Guid, List<Node>> GetParentBindings();
    }
}