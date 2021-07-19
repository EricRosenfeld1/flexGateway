using flexGateway.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using flexGateway.Shared;
using System.Diagnostics;
using flexGateway.Common.Adapter;
using flexGateway.Common.AdapterNode;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using flexGateway.Common.MachineNode;

namespace flexGateway.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NodeController: ControllerBase 
    {
        private IAdapterManager adapterManager;
        private INodeFactory nodeFactory;
        private NodeSynchronizationService nodeSynchroniztaionService;

        public NodeController(IAdapterManager adapterManager, INodeFactory nodeFactory, NodeSynchronizationService service)
        {
            this.adapterManager = adapterManager;
            this.nodeFactory = nodeFactory;
            nodeSynchroniztaionService = service;
        }

        [HttpPost("addNode")]
        public void AddNode(Guid adapterGuid, NodeConfigurationModel nodeModel)  
        {
            var adapter = adapterManager.Publishers.Where(x => x.Guid == adapterGuid).First();
            if(adapter != null)
            {
                var type = nodeFactory.RegisteredTypes.Keys.Where(x => x.FullName == nodeModel.TypeFullName).First();
                var node = nodeFactory.Create(type, nodeModel.Name, Guid.NewGuid(), nodeModel.JsonConfiguration);
                adapter.AddNode(node);
            }
        }

        [HttpGet("getAllTypes")]
        public IEnumerable<NodeConfigurationModel> GetAllTypes()
        {
            var r = new List<NodeConfigurationModel>();

            foreach(var item in nodeFactory.RegisteredTypes.Keys)
            {
                var adaterType = item.FullName;
                var nodeType = nodeFactory.RegisteredTypes[item];
                var configType = nodeFactory.ConfigurationTypes[nodeType];

                r.Add(new NodeConfigurationModel(
                    nodeType.Name,
                    nodeType.FullName,
                    JsonConvert.SerializeObject(Activator.CreateInstance(configType), Formatting.Indented),
                    adaterType));
            }

            return r;
        }
    }
}
