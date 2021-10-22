using flexGateway.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flexGateway.Shared;

namespace flexGateway.Common.Device
{
    public interface IDeviceManager
    {
        public IDevice Source { get; }
        public List<IDevice> Publishers { get; }
        public List<IDevice> Devices { get; }
        public bool AddDevice(IDevice device);
        public bool AddDevice(DeviceConfigurationModel device);
        public bool RemoveDevice(Guid deviceGuid);
    }
}
