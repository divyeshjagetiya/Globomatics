
1) Create Console application in which install Microsoft.Device.Client nuget package.
<br/> And also create one device as "device-01" in Azure IoT Hub. Get ConnectionString from portal.
#
2) We have already created our project locally so now its time to do first commit in Github.
   Use below code in your terminal to push our code.<br/>
a) git init <br/>
b) git status<br/>
c) git add .<br/>
d) git commit -m "first commit"<br/>
e) git remote add origin "github repository link"<br/>
f) git push -u origin master
#
3) Now lets create Development branch so that we will commit our all changes to Development first and then will merge to Master branch
<br/>a) git checkout -b "Development"<br/>
b) git push -u origin Development
#
4) Download IoTHub Explorer from "https://github.com/Azure/iothub-explorer/releases" or Write in Terminal "npm install -g iothub-explorer"
<br/>This will help us to connect our Azure Iot hub from Terminal(cmd).<br/> 
<br/>iothub-explorer help
<br/>iothub-explorer login "HostName='my-hub'.azure-devices.net;SharedAccessKeyName='my-policy';SharedAccessKey='my-policy-key'"
<br/>iothub-explorer login "HostName='my-hub'.azure-devices.net;SharedAccessKeyName='my-policy';SharedAccessKey='my-policy-key'"
<br/>iothub-explorer get known-device --connection-string
<br/>iothub-explorer create new-device --connection-string
<br/>iothub-explorer delete existing-device
#
5) 