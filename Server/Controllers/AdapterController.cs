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

namespace flexGateway.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdapterController : ControllerBase
    {
        private IAdapterManager _adapterManager;
        private IAdapterFactory _adapterFactory;

        public AdapterController(IAdapterFactory adapterFactory, IAdapterManager adapterManager)
        {
            _adapterFactory = adapterFactory;
            _adapterManager = adapterManager;
        }

        [HttpPost("post")]
        public IActionResult Post(AdapterModel adapterModel)
        {
            try
            {
                var type = _adapterFactory.RegisteredTypes.FirstOrDefault(x => x.FullName == adapterModel.TypeAsString);
                if (type is not null)
                {
                    IAdapter adapter = _adapterFactory.Create(type, adapterModel.Name, new Guid(), "");
                    _adapterManager.AddPublisher(adapter);
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
            foreach (var item in _adapterFactory.RegisteredTypes)
                r.Add(new AdapterModel(item.Name, item.FullName, string.Empty));

            return r.ToArray();
        }



    }

}
