using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flexGateway.Common.Adapter
{
    public interface IAdapterManager
    {
        public IAdapter Source { get; }
        public List<IAdapter> Publishers { get; }
        public void AddPublisher(IAdapter publisherAdapter);
        public void RemovePublisher(IAdapter publisherAdapter);
        public T GetAdapter<T>();
    }
}
