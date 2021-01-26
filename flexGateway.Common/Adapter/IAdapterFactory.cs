using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flexGateway.Common.Adapter
{
    public interface IAdapterFactory
    {
        public Dictionary<Type, Type> RegisteredTypes { get; }
        public void Register<T, J>() where T : IAdapter where J : IAdapterConfiguration;
        public IAdapter Create(Type type, IAdapterConfiguration config);
    }
}
