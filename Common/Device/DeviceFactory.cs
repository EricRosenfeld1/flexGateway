using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using flexGateway.Interface;
using Newtonsoft.Json;

namespace flexGateway.Common.Device
{
    public class DeviceFactory : IDeviceFactory
    {
        public Dictionary<Type, Type> RegisteredTypes { get; private set; } = new();

        public void Register(Type deviceType, Type configType)
        {
            if(typeof(IDevice).IsAssignableFrom(deviceType))
                if(typeof(IDeviceConfiguration).IsAssignableFrom(configType))
                    RegisteredTypes.Add(deviceType, configType);
        }

        public IDevice Create(string typeFullName, string configAsJson)
        {
            Type deviceType = RegisteredTypes.Keys.Where(x => x.FullName == typeFullName).First();
            if (deviceType == null)
                throw new Exception("Type not registered");

            Type configType;
            bool success = RegisteredTypes.TryGetValue(deviceType, out configType);

            if (success)
            {
                var config = JsonConvert.DeserializeObject(configAsJson, configType);

                object[] paras = { config };
                var instance = Activator.CreateInstance(deviceType, paras);

                return (IDevice)instance;
            } 
            else
                throw new Exception("Type configuration not found");
        }
    }
}
