using flexGateway.Common.Node;
using flexGateway.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using flexGateway.Shared;

namespace flexGateway.Common.Device
{
    public class DeviceManager : IDeviceManager
    {
        private ILogger<DeviceManager> _logger;
        private IDeviceFactory _deviceFactory;

        private object _lock = new object();
        private List<IDevice> _devices = new List<IDevice>();
        public List<IDevice> Devices 
        {
            get {
                lock (_lock) 
                {
                    return _devices;
                }
            }
            private set {
                {
                    lock (_lock)
                    {
                        _devices = value;
                    }
                }

            }
        }

        public IDevice Source { get; private set; } = null;

        public List<IDevice> Publishers { get; private set; } = new();

        public DeviceManager(ILogger<DeviceManager> logger, IDeviceFactory deviceFactory)
        {
            _logger = logger;
            _deviceFactory = deviceFactory;
        }

        public bool AddDevice(IDevice device)
        {
            if(Devices.Exists(x => x.Name == device.Name)
                || (Source != null && Source.Name == device.Name))
                    return false;

            if (device.IsSource)
                Source = device;
            else
                Publishers.Add(device);

            Devices.Add(device);
            return true;
        }

        public bool AddDevice(DeviceConfigurationModel model)
        {
            try
            {
                IDevice device = _deviceFactory.Create(model.TypeFullName, model.JsonConfiguration);
                device.Guid = Guid.NewGuid();
                device.Name = model.Name;
                device.IsSource = model.IsSource;

                return AddDevice(device);                   
            } 
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
        public bool RemoveDevice(Guid deviceGuid)
        {
            if(Source.Guid == deviceGuid)
            {
                Source = null;
                Devices.RemoveAll(x => x.Guid == deviceGuid);
                return true;
            }
            else
            {
                var index = Publishers.FindIndex(x => x.Guid == deviceGuid);
                if (index >= 0)
                {
                    Publishers.RemoveAt(index);
                    Devices.RemoveAll(x => x.Guid == deviceGuid);
                    return true;
                }
            }
            return false;
        }
    }
}
