using flexGateway.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flexGateway.Common.AdapterNode
{
    public class NodeFactory : INodeFactory
    {
        public HashSet<Type> RegisteredTypes { get; private set; } = new();
        public void Register(Type type)
        {
            if(typeof(INode).IsAssignableFrom(type))
                RegisteredTypes.Add(type);
        }
        public T Create<T>()
        {
            if (RegisteredTypes.Contains(typeof(T)))
                return Activator.CreateInstance<T>();

            else
                throw new Exception("Type not registered");
        }
    }
}
