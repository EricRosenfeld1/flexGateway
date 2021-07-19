using System;

namespace flexGateway.Shared
{
    public class NodeConfigurationModel
    {
        public string Name { get; set; }
        public string TypeFullName { get; set; }
        public string JsonConfiguration { get; set; }
        public string AdapterType { get; set; }

        public NodeConfigurationModel(string name, string typeFullName, string jsonConfiguration, string adapterType)
        {
            Name = name;
            JsonConfiguration = jsonConfiguration;
            TypeFullName = typeFullName;
            AdapterType = adapterType;
        }
    }
}