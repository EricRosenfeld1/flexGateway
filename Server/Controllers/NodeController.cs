using flexGateway.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using flexGateway.Shared;
using System.Diagnostics;

namespace flexGateway.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NodeController: ControllerBase 
    {
        public NodeController()
        {
            Debug.WriteLine("");
        }

        [HttpGet("get")]
        public IEnumerable<NodeModel> Get()
        {
            List<NodeModel> list = new List<NodeModel>();
            list.Add(new NodeModel());

            return list.ToArray();
        }
    }
}
