using System;
using System.Collections.Generic;
using System.Text;

namespace flexGateway.Shared
{
    public class DeviceConfigurationModel
    {
        public string Name { get; set; }
        public string TypeFullName { get; set; }
        public string JsonConfiguration { get; set; }
        public bool IsSource { get; set; }
    }
}
