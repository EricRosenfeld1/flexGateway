using System;
using System.Collections.Generic;
using flexGateway.Plugin.Configuration;

namespace flexGateway.Plugin
{
    /// <summary>
    /// A class used by a <see cref="Adapter"/> which mirrors a state or value of a device
    /// </summary>
    public abstract class Node
    {
        private object _lock = new object();
        private object _value;

        /// <summary>
        /// Gets or sets the name of the <see cref="Node"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the  of the guid of the <see cref="Node"/>
        /// </summary>
        /// 
        public Guid Guid { get; set; }

        public Guid ParentGuid { get; set; }

        public Guid Binding { get; set; }

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

        public bool IsDirty { get; private set; }

        public void UpdateValue(object newValue)
        {
            if(newValue != _value)
            {
                _value = newValue;
                IsDirty = true;
            }
        }

        public void ResetDirtyFlag()
        {
            IsDirty = false;
        }

        public NodeDataType DataType { get; set; }

        public INodeConfiguration Configuration { get; private set; }

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
