using flexGateway.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flexGateway.Common.Repository
{
    public class AdapterRepository : IAdapterRepository
    {
        private readonly LiteDbContext _liteDb;
        public AdapterRepository(LiteDbContext liteDb)
        {
            _liteDb = liteDb;
        }

        public List<AdapterModel> GetAdapters()
        {
            return _liteDb.Context.GetCollection<AdapterModel>("adapters").FindAll().ToList();
        }

        public void InsertAdapter(AdapterModel adapterModel)
        {
            _liteDb.Context.GetCollection<AdapterModel>("adapters").Insert(adapterModel);
        }

        public void Save()
        {
            _liteDb.Context.Commit();
        }
    }
}
