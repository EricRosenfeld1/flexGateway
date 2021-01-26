using flexGateway.Common.Adapter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace flexGateway.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericAdapterController<T> : ControllerBase where T : IAdapter
    {
        private IAdapterManager _adapterManager;

        public GenericAdapterController(IAdapterManager adapterManager)
        {
            _adapterManager = adapterManager;
        }

        [HttpGet]
        private T Get()
        {
            var adapter = _adapterManager.GetByType(typeof(T));
            return (T)adapter;              
        }

        [HttpPost]
        public IActionResult PostSource(T adapter)
        {           
            return Ok();
        }

        [HttpPost]
        public IActionResult PostPublisher(T adapter)
        {
            return Ok();
        }
    }
}
