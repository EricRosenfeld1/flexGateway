using flexGateway.Common.Adapter;
using flexGateway.Common.AdapterNode;
using flexGateway.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace flexGateway.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private IAdapterFactory _adapterFactory;
        private INodeFactory _nodeFactory;
        public TypeController(IAdapterFactory adapterFactory, INodeFactory nodeFactory)
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


        [HttpGet("getnodetypes")]
        public IEnumerable<TypeModel> GetNodes()
        {
            var types = _nodeFactory.RegisteredTypes.ToList<Type>();
            List<TypeModel> models = new();

            foreach (Type type in types)
            {
                models.Add(new TypeModel { Name = type.Name });
            }
            return models;
        }

    }
}
