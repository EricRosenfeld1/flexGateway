using flexGateway.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MockingAdapter
{
    public class MockConfiguration : IAdapterConfiguration
    {
        public MockConfiguration()
        {

        }
        public string Name { get; set; }
    }
}
