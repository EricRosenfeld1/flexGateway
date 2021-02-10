using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flexGateway.Interface;

namespace flexGateway.Common.Adapter
{
    public class AdapterFactory : IAdapterFactory
    {
        public HashSet<Type> RegisteredTypes { get; private set; } = new();

        public void Register(Type type)
        {
            if(typeof(IAdapter).IsAssignableFrom(type))
                RegisteredTypes.Add(type);
        }

        public T Create<T>(string name, Guid guid, string config) where T : IAdapter
        {
            if (RegisteredTypes.Contains(typeof(T)))
            {
                object[] paras = { name, guid, config };
                var instance = Activator.CreateInstance(typeof(T), paras);
                return (T)instance;
            }
            else
                throw new Exception("Type not registered");
        }
    }
}
