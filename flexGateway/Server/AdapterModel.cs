using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace flexGateway.Server
{
    public class AdapterModel
    {
        public string Name { get; set; }
        public Type AdapterType { get; set; }
        public string ConfigurationData { get; set; }
    }
}
