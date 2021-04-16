using System;
using System.Collections.Generic;
using System.Text;

namespace flexGateway.Shared
{
    public class NodeModel
    {
        public NodeModel(string name, string type, string config)
        {
            Name = name;
            JsonConfiguration = config;
            TypeAsString = type;
        }
        public NodeModel()
        {
        }

        public string TypeAsString { get; set; }
        public string Name { get; set; }
        public string JsonConfiguration { get; set; }
    }
}
