using flexGateway.Interface;
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

        public void AddSource(IAdapter sourceAdapter)
        {
            if(_registeredTypes.TryAdd(sourceAdapter.GetType(), sourceAdapter))
                Source = sourceAdapter;
        }

        public void RemoveAdapter(Guid adapterGuid)
        {
            Type type = null;
            if(Source.Guid == adapterGuid)
            {
                type = Source.GetType();
                Source = null;
            }
            else
            {
                var index = Publishers.FindIndex(x => x.Guid == adapterGuid);
                if (index >= 0)
                {
                    type = Publishers[index].GetType();
                    Publishers.RemoveAt(index);
                }
            }
            if(type is not null)
                _registeredTypes.Remove(type);
        }

        public T GetAdapter<T>()
        {
            return (T)_registeredTypes[typeof(T)];
        }

    }
}
