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
            FullName = type;
            JsonConfiguration = config;
        }
        public AdapterModel()
        {

        }

        /// <summary>
        /// Representative type name; <see cref="Type.FullName"/>
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// JSON formatted configuration object
        /// </summary>
        public string JsonConfiguration { get; set; }

        /// <summary>
        /// Display name
        /// </summary>
        public string Name { get; set; }
    }
}
