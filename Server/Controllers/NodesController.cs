using flexGateway.Common.Adapters;
using flexGateway.Common.Nodes;
using flexGateway.Plugin;
using flexGateway.Server.Hubs;
using flexGateway.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace flexGateway.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NodesController : ControllerBase
    {
        private IAdapterManager _adapterManager;
        private INodeFactory _nodeFactory;
        private NodeSynchronizationService _nodeSynchroniztaionService;

        public NodesController(IAdapterManager adapterManger, INodeFactory nodeFactory,
            NodeSynchronizationService service)
        {
            _adapterManager = adapterManger;
            _nodeFactory = nodeFactory;
            _nodeSynchroniztaionService = service;
        }

        // POST: api/{deviceGuid}/nodes
        [HttpPost("{deviceGuid}/nodes")]
        public IActionResult AddNode(Guid adapterGuid, NodeConfigurationModel configModel)
        {
            try
            {
                if (_nodeSynchroniztaionService.IsRunning)
                    _nodeSynchroniztaionService.StopAsync(new System.Threading.CancellationToken());

                var adapter = _adapterManager.Adapters.Where(x => x.Guid == adapterGuid).FirstOrDefault();

                if (adapter == null)
                    return BadRequest($"Adapter with guid: '{adapterGuid}' not found.");
                
                var node = _nodeFactory.Create(configModel.TypeFullName, configModel.JsonConfiguration);
                node.Name = configModel.Name;
                node.Guid = Guid.NewGuid();
                node.DataType = NodeDataType.Int;
                node.ParentGuid = configModel.ParentGuid;

                adapter.AddNode(node);                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                if (!_nodeSynchroniztaionService.IsRunning)
                    _nodeSynchroniztaionService.StartAsync(new System.Threading.CancellationToken());
            }
            return Ok();
        }

    }
}
