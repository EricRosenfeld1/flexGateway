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

namespace flexGateway.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdapterController : ControllerBase
    {
        private IAdapterManager _adapterManager;
        private IAdapterFactory _adapterFactory;

        public AdapterController(IAdapterManager adapterManager, IAdapterFactory adapterFactory)
        {
            _adapterManager = adapterManager;
            _adapterFactory = adapterFactory;
        }

        [HttpPost("post")]
        public IActionResult Post(AdapterModel adapterModel)
        {
            try
            {
                var type = _adapterFactory.RegisteredTypes.FirstOrDefault(x => x.Name == adapterModel.TypeModel.Name);
                if (type is not null)
                {
                    IAdapter adapter = _adapterFactory.Create(type, adapterModel.Name, new Guid(), "");
                    _adapterManager.AddPublisher(adapter);
                    return Ok();
                }
                else
                    throw new Exception("Type is not registered");
            }
            catch(Exception ex)
            {
                return Conflict(ex.Message);
            }              
        }
    }

}
