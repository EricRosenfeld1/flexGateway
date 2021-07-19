using DotNetSiemensPLCToolBoxLibrary.Communication;
using flexGateway.Interface;
using Newtonsoft.Json;
using System;

namespace Sinumerik840d
{
    public class Sinumerik840dNode : INode
    {
        public NC_Var NCVar;

        public string Configuration { get; private set; }
        public Guid Guid { get; private set; }
        public string NodeName { get; set; }
        public object Value { get; set; }
        public INode ParentNode { get; set; }
        public NodeDataType NodeType { get; set; }
        public Sinumerik840dNode(string name, Guid guid, string configAsJson) 
        {
            NodeName = name;
            Guid = guid;

            Configuration = configAsJson;
            NCVar = JsonConvert.DeserializeObject<NC_Var>(configAsJson);
        }
    }
}
