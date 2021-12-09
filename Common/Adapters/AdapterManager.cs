using flexGateway.Common.Nodes;
using flexGateway.Common.Repository;
using flexGateway.Plugin;
using flexGateway.Shared;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace flexGateway.Common.Adapters
{
    public class AdapterManager : IAdapterManager
    {
        private ILogger<AdapterManager> _logger;
        private IAdapterFactory _adapterFactory;
        private INodeFactory _nodeFactory;
        private IAdapterRepository _adapterRepo;

        private List<Adapter> _adapters = new List<Adapter>();
        private object _adapterLock = new object();

        /// <summary>
        /// List of <see cref="Adapter"/>
        /// </summary>
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

        public AdapterManager(ILogger<AdapterManager> logger, IAdapterFactory adapterFactory, IAdapterRepository adapterRepository, INodeFactory nodeFactory)
        {
            _logger = logger;
            _adapterFactory = adapterFactory;
            _adapterRepo = adapterRepository;
            _nodeFactory = nodeFactory;

            LoadAdapters();
        }

        public Adapter AddAdapter(Adapter adapter)
        {
            if (Adapters.Exists(x => x.Name == adapter.Name))
                return null;

            if (adapter.IsSource)
                if (Adapters.Any(x => x.IsSource))
                    return null;

            Adapters.Add(adapter);

            var model = new AdapterModel();
            model.Guid = adapter.Guid;
            model.IsSource = adapter.IsSource;
            model.JsonConfiguration = JsonConvert.SerializeObject(adapter.Configuration);
            model.Name = adapter.Name;
            model.TypeFullName = adapter.GetType().FullName;
            model.LastException = adapter.LastException?.Message;

            _adapterRepo.InsertAdapter(model);
            _adapterRepo.Save();

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
            _adapterRepo.DeleteAdapter(adapterGuid);

            if (count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Load all <see cref="AdapterModel"/> from database and add them to <see cref="Adapters"/>
        /// </summary>
        private void LoadAdapters()
        {
            var adapterModels = _adapterRepo.GetAdapters();
            var adapters = new List<Adapter>();
            foreach(var adapterModel in adapterModels)
            {
                try 
                {
                    var adapter = _adapterFactory.Create(adapterModel);

                    if (adapterModel.Nodes != null)
                        foreach (var nodeModel in adapterModel.Nodes)
                            adapter.AddNode(_nodeFactory.Create(nodeModel));   

                    adapters.Add(adapter); 
                }
                catch (Exception ex) { _logger.LogError(ex.Message); }

                Adapters = adapters;
            }
        }
    }
}
