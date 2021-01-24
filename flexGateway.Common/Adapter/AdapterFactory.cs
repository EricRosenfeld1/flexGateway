using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flexGateway.Common.Adapter
{
    public class AdapterFactory
    {
        public Dictionary<Type, Type> RegisteredTypes { get; private set; } = new();

        public void Register<T, J>(string name) where T : IAdapter where J : IAdapterConfiguration
        {
            var adapterType = typeof(T);
            var configType = typeof(J);

            if (adapterType.IsAbstract || adapterType.IsInterface)
                throw new Exception("Cannot create instace of this type");

            if (configType.IsAbstract || configType.IsInterface)
                throw new Exception("Cannot create instace of this type");

            RegisteredTypes.Add(adapterType, configType);
        }

        public IAdapter Create(Type type, IAdapterConfiguration config)
        {
            Type configType;
            if (!RegisteredTypes.TryGetValue(type, out configType))
                throw new Exception("Type not registered");

            if (configType != config.GetType())
                throw new Exception("Type not reistered");
          
            return (IAdapter)Activator.CreateInstance(type, config);
        }
    }
}
