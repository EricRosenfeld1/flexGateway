using flexGateway.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace flexGateway.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericNodeController<T> : ControllerBase where T : INode
    {

    }
}
