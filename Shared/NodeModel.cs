using System;
using System.Collections.Generic;
using System.Text;

namespace flexGateway.Shared
{
    public class NodeModel
    {
        public Guid Guid {get; set;}
        public Guid ParentGuid { get; set; }
        public string Name { get; set; }

        public NodeModel()
        {

        }
    }
}
