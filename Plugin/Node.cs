using System;
using flexGateway.Plugin.Configuration;

namespace flexGateway.Plugin
{
    public abstract class Node
    {
        private object _lock = new object();
        private object _value;

        public Guid Guid { get; set; }
        public string Name { get; set; }
        public Guid ParentGuid { get; set; }
        public object Value
        {
            get
            {
                lock (_lock)
                    return _value;
            }
            set
            {
                lock (_lock)
                    _value = value;
            }
        }
        public NodeDataType DataType { get; set; }
        public INodeConfiguration Configuration { get; set; }

        public virtual void Configure(INodeConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
    public enum NodeDataType
    {
        String = 0,
        Int = 1,
        Double = 2
    }
}
