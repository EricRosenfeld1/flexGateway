using flexGateway.Common.Device;
using flexGateway.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace flexGateway.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceTypesController : ControllerBase
    {
        private IDeviceFactory _deviceFactory;

        public DeviceTypesController(IDeviceFactory deviceFactory)
        {
            _deviceFactory = deviceFactory;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DeviceTypeModel>> GetDeviceTypes()
        {
            var types = new List<DeviceTypeModel>();
            foreach (var item in _deviceFactory.RegisteredTypes.Keys)
                types.Add(new DeviceTypeModel()
                {
                    TypeFullName = item.FullName,
                    JsonConfiguration = JsonConvert.SerializeObject(Activator.CreateInstance(_deviceFactory.RegisteredTypes[item]),
                    Formatting.Indented)
                });

            return types.ToArray();
        }
    }
}
