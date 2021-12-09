using flexGateway.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flexGateway.Common.Repository
{
    public interface IAdapterRepository
    {
        List<AdapterModel> GetAdapters();
        void InsertAdapter(AdapterModel adapterModel);
        void Save();
        void DeleteAdapter(Guid guid);
    }
}
