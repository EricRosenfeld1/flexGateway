using flexGateway.Interface;

namespace Sinumerik840d
{
    public class Siumerik840dConfiguration : IAdapterConfiguration
    {
        public Siumerik840dConfiguration()
        {

        }
        public string IpAdress { get; set; }
        public int Slot { get; set; }
        public int Rack { get; set; }
    }
}
