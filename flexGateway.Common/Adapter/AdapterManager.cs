using flexGateway.Common.MachineNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flexGateway.Common.Adapter
{
    public class AdapterManager
    {
        public IAdapter Source { get; private set; }

        public List<IAdapter> Publishers { get; private set; } = new();

        public void AddPublishers(IAdapter publisherAdapter)
        {
            if (!Publishers.Exists(x => x.Name == publisherAdapter.Name))
                Publishers.Add(publisherAdapter);
        }

        public void RemovePublishers(IAdapter publisherAdapter)
        {
            if (Publishers.Contains(publisherAdapter))
                Publishers.Remove(publisherAdapter);
        }

    }
}
