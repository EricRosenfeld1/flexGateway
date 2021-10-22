using System;
using System.Collections.Generic;
using System.Text;

namespace flexGateway.Shared
{
    public class StartServiceModel
    {
        public string Status { get; set; }
        public string Name { get; set; }
        public StartServiceModel(string status, string name)
        {
            Status = status;
            Name = name;
        }
    }
}
