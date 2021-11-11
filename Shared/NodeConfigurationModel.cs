using System;

namespace flexGateway.Shared
{
    public class NodeConfigurationModel
    {
        public string Name { get; set; }
        public string TypeFullName { get; set; }
        public string JsonConfiguration { get; set; }
        public string DeviceTypeFullName { get; set; }
        public Guid DeviceGuid { get; set; } = Guid.Empty;
        public Guid ParentGuid { get; set; } = Guid.Empty;
    }
}