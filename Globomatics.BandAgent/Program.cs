/* 
 * Divyesh Jagetiya
 * Date: 05-July-2018
 */
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Globomatics.Common;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;

namespace Globomatics.BandAgent
{
    public class Program
    {
        /// <summary>
        /// For Connecting a particular device we required connecting string of that device, which we can get from Azure -> IoT Hub Name -> Device 01 -> "Connecting String"  
        /// </summary>
        private const string DeviceConnectionString = "HostName=DIoTDemo.azure-devices.net;DeviceId=device-01;SharedAccessKey=NuuXlJYWG8nkbg4j4Xzm0x1GzKPYeGd90JB/XaEwX2o=";
        static async Task Main(string[] args)
        {

            Console.WriteLine("Initializing Band Agent ...");
            //DeviceClient will help us to creating a new client using given connecting string.  So lest create client.
            var device = DeviceClient.CreateFromConnectionString(DeviceConnectionString);
            //We have created our client now it's time to connect with the hub. So OpenAsync will help us to connect with HUB.
            await device.OpenAsync();

            //Suppose if we want to send message to our iOt Device then Device should be always in receving mode so we can get notification from IOT Hub
            var receiveEventsTask = ReveiveEvents(device);

            //Suppose if we send a message from cloud to device with Method name and the method is not present then 
            //it will send null as a response but we can set default method.
            await device.SetMethodDefaultHandlerAsync(OtherDeviceMethod, null);

            // And if we have a method then we can call directly.
            //Insted of Cloud to device messaging we can also use Direct method which will call Directly to perticular method.
            //It will used for if we are sending a message to Device then will get conformation message again
            await device.SetMethodHandlerAsync("showMessage", ShowMessage, null);

            Console.WriteLine("Device is Connected!");
            //Our Device is connected now let's put our data into the cloud 
            // Obviously we will not write while(true) in production side, this will be just testing purpose

            await UpdateTwin(device);

            Console.WriteLine("Press a key to perform an action : ");
            Console.WriteLine("q: quits");
            Console.WriteLine("h: send happy feedback");
            Console.WriteLine("u: send unhappy feedback");
            Console.WriteLine("e: request emergency help");

            var random = new Random();
            var quitRequested = false;
            while(!quitRequested)
            {
                Console.Write("Action ? : ");
                var input = Console.ReadKey().KeyChar;
                Console.WriteLine();

                //Currently we dont have real device so we can give random value to latitude and longitude
                var status = StatusType.NotSpecified;
                var latitude = random.Next(0, 100);
                var longitude = random.Next(0, 100);

                //user already given input for status so now we can set status value. 
                switch(Char.ToLower(input))
                {
                    case 'q':
                        quitRequested = true;
                        break;
                    case 'h':
                        status = StatusType.Happy;
                        break;
                    case 'u':
                        status = StatusType.Unhappy;
                        break;
                    case 'e':
                        status = StatusType.Emergency;
                        break;
                    default:
                        quitRequested = true;
                        break;
                }

                //Now we have all the required data which we want to send to IoT hub as a payload. So lets create payload
                var telemetry = new Telemetry
                {
                    Latitute = latitude,
                    Longitude = longitude,
                    Status = status
                };

                var payload = JsonConvert.SerializeObject(telemetry);
                var message = new Message(Encoding.ASCII.GetBytes(payload));
                await device.SendEventAsync(message);
                Console.WriteLine("Message send to IoT hub!");

            }
        }

        private static Task<MethodResponse> OtherDeviceMethod(MethodRequest methodRequest, object userContext)
        {
            //this method will help if we are sending a message from cloud to device with method but if method is
            //not present then we will execute this one.
            Console.WriteLine("***Other Device Method Called***");
            Console.WriteLine($"Method: {methodRequest.Name}");
            Console.WriteLine($"Payload: {methodRequest.DataAsJson}");

            var responsePayload = Encoding.ASCII.GetBytes("{\"response\": \"This method is not implemented!\"}");

            return Task.FromResult(new MethodResponse(responsePayload, 404));
        }

        private static async Task ReveiveEvents(DeviceClient device)
        {
            //We have to notification from IOT hub so loop should be in alert mode everytime
            while(true)
            {
                //ReceiveAsync will help to receive message whenever we will get any message from hub.
                var message = await device.ReceiveAsync();
                if(message == null)
                {
                    continue;
                }
                //If we didn't get any message then it will continue but suppose if we got then we have display that message as of now.
                var messageBody = message.GetBytes();

                //The message will be in bytes of array so we need to convert that into string 
                var payload = Encoding.ASCII.GetString(messageBody);
                Console.WriteLine($"Receive message from cloud: '{payload}'");

                //Once we received message then we have to inform to IoT Hub that we received message from your side.
                //Here we are using AMQT in which we can send 3feedback like 1) Message Accepted 2) Message Rejected 3) Message Abandon (it will send again and again message until we accept or reject)
                //Suppose we failed to process sucessfuly then we have to Reject the message using "device.RejectAsync(message)"
                //But here we are accepting the message.
                await device.CompleteAsync(message);
            }
        }

        private static async Task UpdateTwin(DeviceClient device)
        {
            //TwinProperties will ussed for the generating new property for the device
            //Twinconllection is the dictionary of key and value pair
            var twinProperties = new TwinCollection();
            twinProperties["connectionType"] = "wi-fi";
            twinProperties["connectionStrength"] = "full";

            await device.UpdateReportedPropertiesAsync(twinProperties);
        }

        private static Task<MethodResponse> ShowMessage(MethodRequest methodRequest , object userContext)
        {
            //If we were writing this to run on real hardware, we'd take the payload and use it to call some API that
            //would show the message on our dand's display. But since we aren't running on real hardware, we'll just log
            //the payload to the console.That payload is available both as bytes, and available as JSON data in this DataAsJson property.
            Console.WriteLine("***Message Received***");
            Console.WriteLine(methodRequest.DataAsJson);

            //We still need to reply back to the colud. Let's first make a new response payload, which is a JSON object
            //containing whaterver we want. For the sake of simplicity, we'll just hand back a simple string message.

            var responsePayload = Encoding.ASCII.GetBytes("{\"response\": \"Message shown!\"}");
            return Task.FromResult(new MethodResponse(responsePayload, 200));
        }
    }
}
 