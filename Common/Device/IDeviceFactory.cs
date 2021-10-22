using flexGateway.Interface;
using System;
using System.Collections.Generic;

namespace flexGateway.Common.Device
{
    public interface IDeviceFactory
    {
        /// <summary>
        /// All registered device types including their configuration type
        /// </summary>
        Dictionary<Type, Type> RegisteredTypes { get; }
        IDevice Create(string typeFullName, string configAsJson);
        void Register(Type deviceType, Type configType);
    }
}