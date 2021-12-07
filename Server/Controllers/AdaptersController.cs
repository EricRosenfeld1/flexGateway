using flexGateway.Common.Adapters;
using flexGateway.Common.Nodes;
using flexGateway.Plugin;
using flexGateway.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace flexGateway.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdaptersController : ControllerBase
    {
        private readonly IAdapterManager _adapterManager;
        private readonly NodeSynchronizationService _nodeSynchroniztaionService;

        public AdaptersController(IAdapterManager adapterManager, NodeSynchronizationService nodeSynchroniztaionService)
        {
            _adapterManager = adapterManager;
            _nodeSynchroniztaionService = nodeSynchroniztaionService;
        }

        // POST: api/adapters
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PostAdapter(AdapterConfigurationModel adapterModel)
        {
            var adapter = AddAdapter(adapterModel);
            if (adapter != null)
                return CreatedAtAction(nameof(GetAdapter), new { guid = adapter.Guid}, adapter);
            else
                return NoContent();
        }

        // GET: api/adapters
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<AdapterModel>> GetAdapters()
        {
            var r = new List<AdapterModel>();
            foreach (var item in _adapterManager.Adapters)
            {
                var nodes = new List<NodeModel>();
                foreach (var node in item.GetNodes())
                {
                    nodes.Add(new NodeModel()
                    {
                        Name = node.Name,
                        Guid = node.Guid,
                        ParentGuid = node.ParentGuid
                    });
                }

                r.Add(new AdapterModel()
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

            return Ok(r.ToArray());
        }

        // GET: api/adapters/{guid}
        [HttpGet("{guid}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AdapterModel> GetAdapter(Guid guid)
        {
            var adapter = _adapterManager.Adapters.Where(x => x.Guid == guid).FirstOrDefault();
            if (adapter == null)
                return NotFound($"Adapter with guid '{guid}' not found.");

            var model = new AdapterModel()
            {
                Name = adapter.Name,
                Guid = adapter.Guid,
                IsSource = adapter.IsSource,
                TypeFullName = adapter.GetType().FullName,
                LastException = adapter.LastException?.ToString(),
                IsConnected = adapter.IsConnected
            };

            model.Nodes = new List<NodeModel>();
            foreach (var node in adapter.GetNodes())
            {
                model.Nodes.Add(new NodeModel()
                {
                    Name = node.Name,
                    Guid = node.Guid,
                    NodeType = node.DataType,
                    Value = node.Value,
                    ParentGuid = node.ParentGuid
                });
            }

            return Ok(model);
        }

        // DELETE: api/adapters/{guid}
        [HttpDelete("{guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteAdatper(Guid guid)
        {
            var success = _adapterManager.RemoveAdapter(guid);
            return Ok(success);
        }

        private Adapter AddAdapter(AdapterConfigurationModel adpaterModel)
        {
            try
            {
                if (_nodeSynchroniztaionService.IsRunning)
                    _nodeSynchroniztaionService.StopAsync(new System.Threading.CancellationToken());

                return _adapterManager.AddAdapter(adpaterModel);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (!_nodeSynchroniztaionService.IsRunning)
                    _nodeSynchroniztaionService.StartAsync(new System.Threading.CancellationToken());
            }
        }
    }
}
