using flexGateway.Common.Adapters;
using flexGateway.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace flexGateway.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdapterTypesController : ControllerBase
    {
        private IAdapterFactory _deviceFactory;

        public AdapterTypesController(IAdapterFactory deviceFactory)
        {
            _deviceFactory = deviceFactory;
        }

        // GET: api/adaptertypes
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<AdapterTypeModel>> GetDeviceTypes()
        {
            var types = new List<AdapterTypeModel>();
            foreach (var item in _deviceFactory.RegisteredTypes.Keys)
                types.Add(new AdapterTypeModel()
                {
                    TypeFullName = item.FullName,
                    JsonConfiguration = JsonConvert.SerializeObject(Activator.CreateInstance(_deviceFactory.RegisteredTypes[item]),
                    Formatting.Indented)
                });

            return Ok(types.ToArray());
        }
    }
}
