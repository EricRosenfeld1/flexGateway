using flexGateway.Plugin.Configuration;

namespace MockingAdapter
{
    public class MockNodeConfiguration : INodeConfiguration
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
