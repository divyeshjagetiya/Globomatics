
1) Create Console application in which install Microsoft.Device.Client nuget package.
#
2) We have already created our project locally so now its time to do first commit in Github.
   Use below code in your terminal to push our code.
a) git init 
b) git status
c) git add .
d) git commit -m "first commit"
e) git remote add origin "github repository link"
f) git push -u origin master
#
3) Now lets create Development branch so that we will commit our all changes to Development first and then will merge to Master branch
a) git checkout -b "Development"
b) git push -u origin Development
#
4) Download IoTHub Explorer from "https://github.com/Azure/iothub-explorer/releases" or Write in Terminal "npm install -g iothub-explorer"
<br/><br/>iothub-explorer help
<br/>iothub-explorer login "HostName='my-hub'.azure-devices.net;SharedAccessKeyName='my-policy';SharedAccessKey='my-policy-key'"
<br/>iothub-explorer login "HostName='my-hub'.azure-devices.net;SharedAccessKeyName='my-policy';SharedAccessKey='my-policy-key'"
<br/>iothub-explorer get known-device --connection-string
<br/>iothub-explorer create new-device --connection-string
<br/>iothub-explorer delete existing-device

5) 