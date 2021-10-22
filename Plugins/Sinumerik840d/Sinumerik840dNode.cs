using DotNetSiemensPLCToolBoxLibrary.Communication;
using flexGateway.Interface;
using Newtonsoft.Json;
using System;

namespace Sinumerik840d
{
    public class Sinumerik840dNode : INode
    {
        public INodeConfiguration Configuration { get; private set; }
        public Guid Guid { get; set; }
        public string NodeName { get; set; }
        public object Value { get; set; }
        public Guid ParentGuid { get; set; }
        public NodeDataType NodeType { get; set; }

        public NC_Var NCVar;

        public Sinumerik840dNode(Sinumerik840dNodeConfiguration config) 
        {
            Configuration = config;
            NCVar = new NC_Var();
        }
    }
}
