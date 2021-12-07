using flexGateway.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace flexGateway.Common
{
    public class LiteDbAdapterService
    {
        private LiteDB.LiteDatabase _liteDb;

        public LiteDbAdapterService(LiteDbContext context)
        {
            _liteDb = context.Context;
        }

        public List<Adapter> GetAdapters()
        {
           return _liteDb.GetCollection<Adapter>("adapters").FindAll().ToList();
        }
    }
}
