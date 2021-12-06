using System;
using System.ComponentModel.DataAnnotations;

namespace flexGateway.Shared
{
    public class NodeConfigurationModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string TypeFullName { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string JsonConfiguration { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string DeviceTypeFullName { get; set; }

        public Guid DeviceGuid { get; set; } = Guid.Empty;

        public Guid ParentGuid { get; set; } = Guid.Empty;
    }
}