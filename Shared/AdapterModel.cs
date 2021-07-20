using System;
using System.Collections.Generic;
using System.Text;

namespace flexGateway.Shared
{
    public class AdapterModel
    {
        public string Name { get; set; }
        public string TypeFullName { get; set; }
        public Guid Guid { get; private set; }
        public List<NodeModel> Nodes { get; set; }
        public AdapterModel(string name, Guid guid, string typeFullName, List<NodeModel> nodes)
        {
            Name = name;
            Guid = guid;
            TypeFullName = typeFullName;
            Nodes = nodes;
        }

    }
}
