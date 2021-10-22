using flexGateway.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MockingAdapter
{
    public class MockConfiguration : IDeviceConfiguration
    {
        public int Seed { get; set; }
    }
}
