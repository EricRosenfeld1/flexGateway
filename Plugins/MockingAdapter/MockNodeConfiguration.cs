using flexGateway.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MockingAdapter
{
    public class MockNodeConfiguration : INodeConfiguration
    {
        public MockNodeConfiguration()
        {

        }
        public string Name { get; set; }
    }
}
