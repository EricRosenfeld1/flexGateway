using System;
using System.Collections.Generic;
using System.Text;

namespace flexGateway.Shared
{
    public class AdapterConfigurationModel
    {
        public string TypeFullName { get; set; }
        public string JsonConfiguration { get; set; }
        public string Name { get; set; }

        public AdapterConfigurationModel(string name, string typeFullName, string jsonConfiguration)
        {
            Name = name;
            TypeFullName = typeFullName;
            JsonConfiguration = jsonConfiguration;
        }
    }
}
