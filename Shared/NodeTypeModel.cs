using System;
using System.Collections.Generic;
using System.Text;

namespace flexGateway.Shared
{
    public class NodeTypeModel
    {
        public string TypeFullName { get; set; }
        public string JsonConfiguration { get; set; }
        public NodeTypeModel(string typeFullName, string jsonConfiguration)
        {
            TypeFullName = typeFullName;
            JsonConfiguration = jsonConfiguration;
        }
    }
}
