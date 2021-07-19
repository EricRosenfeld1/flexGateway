using System;
using System.Collections.Generic;
using System.Text;

namespace flexGateway.Shared
{
    public class NodeModel
    {
        public Guid Guid{get;set;}
        public NodeModel ParentGuid { get; set; }
        public string Name { get; set; }
        public NodeModel()
        {
        }
    }
}
