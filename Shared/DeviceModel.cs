using System;
using System.Collections.Generic;
using System.Text;

namespace flexGateway.Shared
{
    public class DeviceModel
    {
        public string Name { get; set; }
        public string TypeFullName { get; set; }
        public bool IsSource { get; set; }
        public Guid Guid { get; set; }
        public List<NodeModel> Nodes { get; set; }
        public string LastException { get; set; }
        public bool IsConnected { get; set; }

    }
}
