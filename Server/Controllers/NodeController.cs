using flexGateway.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using flexGateway.Shared;

namespace flexGateway.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NodeController: ControllerBase 
    {
        [HttpPost("post")]
        public IActionResult Post(NodeModel nodeModel)
        {
            return Ok();
        }
    }
}
