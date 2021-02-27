using flexGateway.Common.Adapter;
using flexGateway.Common.AdapterNode;
using flexGateway.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace flexGateway.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private IAdapterFactory _adapterFactory;
        private INodeFactory _nodeFactory;
        public TypesController(IAdapterFactory adapterFactory, INodeFactory nodeFactory)
        {
            _adapterFactory = adapterFactory;
            _nodeFactory = nodeFactory;
        }

        [HttpGet("getadaptertypes")]
        public IEnumerable<TypeModel> GetAdapters()
        {
            var types = _adapterFactory.RegisteredTypes.ToList<Type>();
            List<TypeModel> models = new();

            foreach(Type type in types)
            {
                models.Add(new TypeModel { Name = type.Name });
            }
            return models;
        }

    }
}
