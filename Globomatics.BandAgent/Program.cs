/* 
 * Divyesh Jagetiya
 * Date: 05-July-2018
 * Code followed from https://app.pluralsight.com/player?course=azure-iot-hub-developers-getting-started&author=matt-honeycutt&name=a7db97d2-7d1e-40d1-8acd-a889e1f17087&clip=1&mode=live
 */
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;

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

            Console.WriteLine("Press Key to exit..");
            Console.ReadKey();



        }
    }
}
