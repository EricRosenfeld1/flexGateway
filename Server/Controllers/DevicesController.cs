using flexGateway.Common.Device;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using flexGateway.Shared;
using Newtonsoft.Json;
using flexGateway.Common.Node;

namespace flexGateway.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceManager _deviceManager;
        private readonly NodeSynchronizationService _nodeSynchroniztaionService;

        public DevicesController(IDeviceManager deviceManager, NodeSynchronizationService nodeSynchroniztaionService)
        {
            _deviceManager = deviceManager;
            _nodeSynchroniztaionService = nodeSynchroniztaionService;
        }

        [HttpPost]
        public IActionResult PostDevice(DeviceConfigurationModel deviceModel)
        {
            var guid = AddDevice(deviceModel);
            if (guid != Guid.Empty)
                return Ok(guid);
            else
                return UnprocessableEntity();
        }

        [HttpGet]
        public ActionResult<IEnumerable<DeviceModel>> GetDevices()
        {
            var r = new List<DeviceModel>();
            foreach (var item in _deviceManager.Devices)
            {
                var nodes = new List<NodeModel>();
                if (item.GetNodes().Count > 0)
                    foreach (var node in item.GetNodes())
                    {
                        nodes.Add(new NodeModel()
                        {
                            Name = node.NodeName,
                            Guid = node.Guid,
                            ParentGuid = node.ParentGuid
                        });                  
                }

                r.Add(new DeviceModel()
                {
                    Name = item.Name,
                    Guid = item.Guid,
                    IsSource = item.IsSource,
                    TypeFullName = item.GetType().FullName,
                    LastException = item.LastException?.ToString(),
                    IsConnected = item.IsConnected,
                    Nodes = nodes
                });
            }

            return r.ToArray();
        }

        [HttpGet("{guid}")]
        public ActionResult<DeviceModel> GetDevice(Guid guid)
        {
            var device = _deviceManager.Devices.Where(x => x.Guid == guid).FirstOrDefault();
            if (device == null)
                return NotFound("Device not found.");

            var model = new DeviceModel()
            {
                Name = device.Name,
                Guid = device.Guid,
                IsSource = device.IsSource,
                TypeFullName = device.GetType().FullName,
                LastException = device.LastException?.ToString(),
                IsConnected = device.IsConnected
            };

            if(device.GetNodes().Count > 0)
            {
                model.Nodes = new List<NodeModel>();
                foreach(var node in device.GetNodes())
                {
                    model.Nodes.Add(new NodeModel()
                    {
                        Name = node.NodeName,
                        Guid = node.Guid,
                        NodeType = node.NodeType,
                        Value = node.Value,
                        ParentGuid = node.ParentGuid
                    });
                }
            }

            return model;
        }

        [HttpDelete("{guid}")]
        public IActionResult DeleteDevice(Guid guid)
        {
            var success = _deviceManager.RemoveDevice(guid);
            return Ok(success);
        }

        private Guid AddDevice(DeviceConfigurationModel deviceModel)
        {
            try
            {
                if (_nodeSynchroniztaionService.IsRunning)
                    _nodeSynchroniztaionService.StopAsync(new System.Threading.CancellationToken());

                return _deviceManager.AddDevice(deviceModel);              
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
            finally
            {
                if (!_nodeSynchroniztaionService.IsRunning)
                    _nodeSynchroniztaionService.StartAsync(new System.Threading.CancellationToken());
            }
        }
    }
}
