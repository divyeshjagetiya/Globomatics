
1) Create Console application in which install Microsoft.Device.Client nuget package.

2) Download IoTHub Explorer from "https://github.com/Azure/iothub-explorer/releases" or Write in Terminal "npm install -g iothub-explorer"
iothub-explorer help
iothub-explorer login "HostName=<my-hub>.azure-devices.net;SharedAccessKeyName=<my-policy>;SharedAccessKey=<my-policy-key>"
iothub-explorer get known-device --connection-string
iothub-explorer create new-device --connection-string
iothub-explorer delete existing-device

3) 