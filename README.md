# flexGateway

**THIS SOFTWARE IS NOT (ready) FOR PRODUCTION USE**

A Raspberry-Pi based industrial gateway. It is designed to enable IoT (MQTT, OPC, ...) ability to industrial machines.

The adapters provide and handle the machine/hardware/software specific implementation which allow you to read or write values to your device. 
Due to the 1:n connection like manner, you can configure the communication within the gateway to your needs.
The plugin system can be used to write your own adapter implementation.

*Example:*
```
                                         |<--> OPC Server  <--> Company OEE / ERP  
 [CNC Siemens 840dsl] Parameter "R10" ---|---> SQL Adapter 
                                         |---> MQTT       
                                        
```

The gateway will be controlled through a Blazor WebAssembly app.

## Adapters
### Available Adapters
- Siemens Sinumerik 840dsl (NCK variables, GUD, R-Parameter, tool data, upload/download nc programs)

### Planned Adapters
- Okuma Open API 
- Okuma OSP-P
- OPC
- MQTT
- WebApi
- Zeiss Calypso 
- Siemens S7-300/400 and S5 PLCs
- Raspberry Pi GPIO / Sensors

## Thanks To
- https://github.com/dotnetprojects/DotNetSiemensPLCToolBoxLibrary for providing the c# wrapper of the libnodave lib and general siemens plc communication.

## Project Information

If you are interested in IoT, Indsutry 4.0 or general industriual machiniery feel free to contribute to my project and help me with the implementation.
