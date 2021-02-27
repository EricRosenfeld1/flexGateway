using System;
using System.Collections.Generic;
using System.Text;

namespace flexGateway.Shared
{
    public class NodeModel
    {
        public NodeModel(string name, TypeModel typeModel, string config)
        {
            Name = name;
            JsonConfiguration = config;
            TypeModel = typeModel;
        }
        public NodeModel()
        {
        }

        public TypeModel TypeModel { get; set; }
        public string Name { get; set; }
        public string JsonConfiguration { get; set; }
    }
}
