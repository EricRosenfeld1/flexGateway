using flexGateway.Plugin;
using flexGateway.Plugin.Configuration;

namespace MockingAdapter
{
    public class MockNode : Node
    {
        public int Max { get; set; }
        public int Min { get; set; }

        public override void Configure(INodeConfiguration configuration)
        {
            var c = configuration as MockNodeConfiguration;
            Max = c.Max;
            Min = c.Min;
            base.Configure(configuration);
        }
    }
}
