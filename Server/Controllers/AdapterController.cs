using flexGateway.Common.Adapter;
using flexGateway.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using flexGateway.Common;
using flexGateway.Shared;
using System.Diagnostics;
using Newtonsoft.Json;
using flexGateway.Common.MachineNode;
using Microsoft.Extensions.Hosting;

namespace flexGateway.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdapterController : ControllerBase
    {
        private IAdapterManager adapterManager;
        private IAdapterFactory adapterFactory;
        private NodeSynchronizationService nodeSynchroniztaionService;

        public AdapterController(IAdapterFactory adapterFactory, IAdapterManager adapterManager, NodeSynchronizationService service)
        {
            this.adapterFactory = adapterFactory;
            this.adapterManager = adapterManager;

            nodeSynchroniztaionService = service;
        }

        [HttpPost("postSouce")]
        public IActionResult PostSouce(AdapterConfigurationModel adapterModel)
        {
            var success = AddAdapter(adapterModel, true);
            if (success)
                return Ok();
            else
                return Conflict();
        }

        [HttpPost("postPublisher")]
        public IActionResult PostPublisher(AdapterConfigurationModel adapterModel)
        {
            var success = AddAdapter(adapterModel, false);
            if (success)
                return Ok();
            else
                return Conflict();
        }

        [HttpGet("getAllTypes")]
        public IEnumerable<AdapterConfigurationModel> GetAllTypes()
        {
            var r = new List<AdapterConfigurationModel>();
            foreach (var item in adapterFactory.RegisteredTypes.Keys)
                r.Add(new AdapterConfigurationModel(
                    item.Name, 
                    item.FullName, 
                    JsonConvert.SerializeObject(Activator.CreateInstance(adapterFactory.RegisteredTypes[item]), 
                    Formatting.Indented)));

            return r.ToArray();
        }

        [HttpGet("getPublishers")]
        public IEnumerable<AdapterModel> GetPublishers()
        {
            var r = new List<AdapterModel>();
            foreach (var item in adapterManager.Publishers)
            {
                var nodes = new List<NodeModel>();
                foreach (var node in item.Nodes)
                {
                    nodes.Add(new NodeModel()
                    {
                        Name = node.NodeName,
                        Guid = node.Guid,
                        ParentGuid = node.ParentNode.Guid
                    });
                }

                r.Add(new AdapterModel(item.Name, item.Guid, item.GetType().FullName, nodes));
            }
                

            return r.ToArray();
        }

        [HttpGet("getSource")]
        public AdapterModel GetSource()
        {
            if (adapterManager.Source != null)
            {
                var source = adapterManager.Source;
                var nodes = new List<NodeModel>();
                foreach(var node in source.Nodes)
                {
                    nodes.Add(new NodeModel()
                    {
                        Name = node.NodeName,
                        Guid = node.Guid,
                        ParentGuid = node.ParentNode.Guid
                    });
                }
                return new AdapterModel(source.Name, source.Guid, source.GetType().FullName, nodes);
            } 
            else
                return null;
        }

        private bool AddAdapter(AdapterConfigurationModel adapterModel, bool isSource)
        {
            try
            {
                if (nodeSynchroniztaionService.IsRunning)
                    nodeSynchroniztaionService.StopAsync(new System.Threading.CancellationToken());

                var type = adapterFactory.RegisteredTypes.Keys.Where(x => x.FullName == adapterModel.TypeFullName).First();
                if (type is not null)
                {
                    IAdapter adapter = adapterFactory.Create(type, adapterModel.Name, Guid.NewGuid(), adapterModel.JsonConfiguration);
                    adapterManager.AddPublisher(adapter);
                    return true;
                }
                else
                    throw new Exception("Type is not registered");
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (!nodeSynchroniztaionService.IsRunning)
                    nodeSynchroniztaionService.StartAsync(new System.Threading.CancellationToken());
            }
        }

    }

}
