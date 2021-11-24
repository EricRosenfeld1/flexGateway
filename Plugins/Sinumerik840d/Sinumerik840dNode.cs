using DotNetSiemensPLCToolBoxLibrary.Communication;
using flexGateway.Plugin;
using flexGateway.Plugin.Configuration;

namespace Sinumerik840d
{
    public class Sinumerik840dNode : Node
    {
        public NC_Var NCVar;

        public override void Configure(INodeConfiguration configuration)
        {
            var c = configuration as Sinumerik840dNodeConfiguration;
            NCVar = new NC_Var();

            base.Configure(configuration);
        }
    }
}
