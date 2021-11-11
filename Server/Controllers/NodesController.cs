using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using flexGateway.Shared;
using flexGateway.Common.Device;
using flexGateway.Common.Node;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using flexGateway.Server.Hubs;
using flexGateway.Interface;

namespace flexGateway.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodesController : ControllerBase 
    {
        private IDeviceManager deviceManager;
        private INodeFactory nodeFactory;
        private NodeSynchronizationService nodeSynchroniztaionService;
        private readonly IHubContext<ServiceHub, IServiceHub> hubContext;

        public NodesController(IDeviceManager deviceManager, INodeFactory nodeFactory, 
            NodeSynchronizationService service)
        {
            this.deviceManager = deviceManager;
            this.nodeFactory = nodeFactory;
            nodeSynchroniztaionService = service;
            hubContext = serviceHub;
        }

        [HttpPost("{deviceGuid}/nodes")]
        public IActionResult AddNode(Guid deviceGuid, NodeConfigurationModel configModel)  
        {         
            try
            {
                if (nodeSynchroniztaionService.IsRunning)
                    nodeSynchroniztaionService.StopAsync(new System.Threading.CancellationToken());

                var device = deviceManager.Devices.Where(x => x.Guid == deviceGuid).FirstOrDefault();

                if (device != null)
                {                   
                    var node = nodeFactory.Create(configModel.TypeFullName, configModel.JsonConfiguration);
                    node.NodeName = configModel.Name;
                    node.Guid = Guid.NewGuid();
                    node.NodeType = NodeDataType.Int;
                    node.ParentGuid = Guid.Empty;
                    
                    device.AddNode(node);
                }
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if (!nodeSynchroniztaionService.IsRunning)
                    nodeSynchroniztaionService.StartAsync(new System.Threading.CancellationToken());
            }
            return Ok();        
        }

        [HttpGet("getNodeType")]
        public NodeTypeModel GetNodeType(Guid DeviceGuid)
        {
            var device = deviceManager.Devices.Where(x => x.Guid == DeviceGuid).FirstOrDefault();
            if (device == null)
                return null;

            var nodeType = nodeFactory.RegisteredTypes[device.GetType()];
            if (nodeType == null)
                return null;

            var configType = nodeFactory.ConfigurationTypes[nodeType];
            if (configType == null)
                return null;

             return new NodeTypeModel(
                    configType.FullName,
                    JsonConvert.SerializeObject(Activator.CreateInstance(configType), Formatting.Indented));
        }

        [HttpGet("getService")]
        public async Task<IEnumerable<StartServiceModel>> GetService()
        {
            var list = new List<StartServiceModel>();
            await hubContext.Clients.All.StatusUpdate("Hello World");
            if (!nodeSynchroniztaionService.IsRunning)
            {
                await nodeSynchroniztaionService.StartAsync(new System.Threading.CancellationToken());
                list.Add(new StartServiceModel("START", "WORLD"));
            }
            else
                list.Add(new StartServiceModel("STOP", "WORLD"));

            return list.ToArray();
        }

    }
}
