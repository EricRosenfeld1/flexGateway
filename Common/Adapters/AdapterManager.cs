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

        public AdapterManager(ILogger<AdapterManager> logger, IAdapterFactory adapterFactory)
        {
            _logger = logger;
            _adapterFactory = adapterFactory;
        }

        public bool AddAdapter(Adapter adapter)
        {
            if (Adapters.Exists(x => x.Name == adapter.Name))
                return false;

            if (adapter.IsSource)
                if (Adapters.Any(x => x.IsSource))
                    return false;

            Adapters.Add(adapter);
            return true;
        }

        public Guid AddAdapter(AdapterConfigurationModel model)
        {
            try
            {
                Adapter adapter = _adapterFactory.Create(model.TypeFullName, model.JsonConfiguration);
                adapter.Guid = Guid.NewGuid();
                adapter.Name = model.Name;
                adapter.IsSource = model.IsSource;

                if (AddAdapter(adapter))
                    return adapter.Guid;
                else
                    return Guid.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Guid.Empty;
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
