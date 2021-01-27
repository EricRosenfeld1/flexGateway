using flexGateway.Common.MachineNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flexGateway.Common.Adapter
{
    public class AdapterManager : IAdapterManager
    {
        private Dictionary<Type, IAdapter> _registeredTypes = new();

        public IAdapter Source { get; private set; }
        public List<IAdapter> Publishers { get; private set; } = new();

        public void AddPublisher(IAdapter publisherAdapter)
        {
            if(_registeredTypes.TryAdd(publisherAdapter.GetType(), publisherAdapter))
                if (!Publishers.Exists(x => x.Name == publisherAdapter.Name))
                Publishers.Add(publisherAdapter);
        }

        public void RemovePublisher(IAdapter publisherAdapter)
        {
            _registeredTypes.Remove(publisherAdapter.GetType());

            if (Publishers.Contains(publisherAdapter))
                Publishers.Remove(publisherAdapter);
        }

        public void AddSource(IAdapter sourceAdapter)
        {
            if(_registeredTypes.TryAdd(sourceAdapter.GetType(), sourceAdapter))
                Source = sourceAdapter;
        }

        public void RemoveSource(IAdapter sourceAdapter)
        {
            _registeredTypes.Remove(sourceAdapter.GetType());

            if(Source is not null)
                Source = null;
        }

        public T GetAdapter<T>()
        {
            return (T)_registeredTypes[typeof(T)];
        }

    }
}
