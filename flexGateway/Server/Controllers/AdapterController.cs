using flexGateway.Common.Adapter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace flexGateway.Server.Controllers
{
    [Route("adapters/[controller]")]
    [ApiController]
    public class GenericAdapterController<T> : ControllerBase where T : IAdapter
    {
        private IAdapterManager adapterManager;

        public GenericAdapterController(IAdapterManager adapterManager)
        {
            adapterManager = this.adapterManager;
        }

        [HttpGet]
        private T Get()
        {
            var adapter = adapterManager.GetAdapter<T>();
            return adapter;              
        }

        [HttpPost]
        public IActionResult PostSource(T adapter)
        {
            try
            {
                adapterManager.AddPublisher(adapter);
                return Ok();
            }
            catch(Exception ex)
            {
                return Conflict();
            }              
        }

        [HttpPost]
        public IActionResult PostPublisher(T adapter)
        {
            return Ok();
        }
    }
}
