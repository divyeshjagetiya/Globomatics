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

        private static async Task UpdateTwin(DeviceClient device)
        {
            //TwinProperties will ussed for the generating new property for the device
            //Twinconllection is the dictionary of key and value pair
            var twinProperties = new TwinCollection();
            twinProperties["connectionType"] = "wi-fi";
            twinProperties["connectionStrength"] = "full";

            await device.UpdateReportedPropertiesAsync(twinProperties);
        }
    }
}
