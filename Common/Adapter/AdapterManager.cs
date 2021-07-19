using flexGateway.Common.MachineNode;
using flexGateway.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flexGateway.Common.Adapter
{
    public class AdapterManager : IAdapterManager
    {
        private Dictionary<Type, IAdapter> adapters = new();
        private ILogger<AdapterManager> logger;

        /// <summary>
        /// Source adapter; single source of truth
        /// </summary>
        public IAdapter Source { get; private set; }

        /// <summary>
        /// List of adapters which will be synchronized to the <see cref="Source"/>
        /// </summary>
        public List<IAdapter> Publishers { get; private set; } = new();

        public AdapterManager(ILogger<AdapterManager> logger)
        {
            this.logger = logger;
        }

        public void AddPublisher(IAdapter publisherAdapter)
        {
            if(adapters.TryAdd(publisherAdapter.GetType(), publisherAdapter))
                if (!Publishers.Exists(x => x.Name == publisherAdapter.Name))
                Publishers.Add(publisherAdapter);

       }

        public void AddSource(IAdapter sourceAdapter)
        {
  
            if (adapters.TryAdd(sourceAdapter.GetType(), sourceAdapter))
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
                adapters.Remove(type);

        }

        public T GetAdapter<T>()
        {
            return (T)adapters[typeof(T)];
        }
    }
}
