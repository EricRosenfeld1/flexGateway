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

        [HttpPost("postPublisher")]
        public IActionResult Post(AdapterConfigurationModel adapterModel)
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
                    return Ok();
                }
                else
                    throw new Exception("Type is not registered");
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
            finally
            {
                if (!nodeSynchroniztaionService.IsRunning)
                    nodeSynchroniztaionService.StartAsync(new System.Threading.CancellationToken());
            }
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
                r.Add(new AdapterModel(item.Name, item.Guid, item.GetType().FullName));

            return r.ToArray();
        }

    }

}
