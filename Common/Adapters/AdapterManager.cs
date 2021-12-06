using flexGateway.Plugin;
using flexGateway.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace flexGateway.Common.Adapters
{
    public class AdapterManager : IAdapterManager
    {
        private ILogger<AdapterManager> _logger;
        private IAdapterFactory _adapterFactory;
        private LiteDbContext _liteDb;

        private List<Adapter> _adapters = new List<Adapter>();
        private object _adapterLock = new object();

        public List<Adapter> Adapters
        {
            set
            {
                lock (_adapterLock)
                    _adapters = value;
            }
            get
            {
                lock (_adapterLock)
                    return _adapters;
            }
        }

        public AdapterManager(ILogger<AdapterManager> logger, IAdapterFactory adapterFactory, LiteDbContext liteDb)
        {
            _logger = logger;
            _adapterFactory = adapterFactory;
            _liteDb = liteDb;
        }

        public Adapter AddAdapter(Adapter adapter)
        {
            if (Adapters.Exists(x => x.Name == adapter.Name))
                return null;

            if (adapter.IsSource)
                if (Adapters.Any(x => x.IsSource))
                    return null;

            Adapters.Add(adapter);

            return adapter;
        }

        public Adapter AddAdapter(AdapterConfigurationModel model)
        {
            try
            {
                Adapter adapter = _adapterFactory.Create(model.TypeFullName, model.JsonConfiguration);
                adapter.Guid = Guid.NewGuid();
                adapter.Name = model.Name;
                adapter.IsSource = model.IsSource;

                return AddAdapter(adapter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public bool RemoveAdapter(Guid adapterGuid)
        {
            var count = Adapters.RemoveAll(x => x.Guid == adapterGuid);

            if (count > 0)
                return true;
            else
                return false;
        }
    }
}
