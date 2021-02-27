using System;
using System.Collections.Generic;
using System.Text;

namespace flexGateway.Shared
{
    public class AdapterModel
    {
        public AdapterModel(string name, TypeModel typeModel, string config)
        {
            Name = name;
            TypeModel = typeModel;
            JsonConfiguration = config;
        }
        public AdapterModel()
        {

        }
        public TypeModel TypeModel { get; set; }
        public string JsonConfiguration { get; set; }
        public string Name { get; set; }
    }
}
