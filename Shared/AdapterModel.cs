using System;
using System.Collections.Generic;
using System.Text;

namespace flexGateway.Shared
{
    public class AdapterModel
    {
        public AdapterModel(string name, string type, string config)
        {
            Name = name;
            TypeAsString = type;
            JsonConfiguration = config;
        }
        public AdapterModel()
        {

        }
        public string TypeAsString { get; set; }
        public string JsonConfiguration { get; set; }
        public string Name { get; set; }
    }
}
