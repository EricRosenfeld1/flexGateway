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

        [HttpPost]
        public IActionResult Post(AdapterModel adapterModel)
        {
            try
            {
                var type = adapterFactory.RegisteredTypes.Keys.Where(x => x.FullName == adapterModel.FullName).First();
                if (type is not null)
                {
                    IAdapter adapter = adapterFactory.Create(type, adapterModel.Name, new Guid(), adapterModel.JsonConfiguration);
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
        }

        [HttpGet]
        public IEnumerable<AdapterModel> Get()
        {
            var r = new List<AdapterModel>();
            foreach (var item in adapterFactory.RegisteredTypes)
            {
                r.Add(new AdapterModel(item.Key.Name, item.Key.FullName, JsonConvert.SerializeObject(Activator.CreateInstance(item.Value), Formatting.Indented)));
            }
            return r.ToArray();
        }

        [HttpGet("start")]
        public IActionResult GetStart()
        {
            nodeSynchroniztaionService.StartAsync(new System.Threading.CancellationToken());
            return Ok();
        }

        [HttpGet("stop")]
        public IActionResult GetStop()
        {
            nodeSynchroniztaionService.StopAsync(new System.Threading.CancellationToken());
            return Ok();
        }

    }

}
