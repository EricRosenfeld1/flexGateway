using flexGateway.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MockingAdapter
{
    public class MockNodeConfiguration : INodeConfiguration
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
