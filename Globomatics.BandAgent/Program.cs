/* 
 * Divyesh Jagetiya
 * Date: 05-July-2018
 */
using System;
using System.Text;
using System.Threading;
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
            //Our Device is connected now let's put our data into the cloud 
            // Obviously we will not write while(true) in production side, this will be just testing purpose
            while (true)
            {
                //For Sending a message to cloud we have to Create instance of Message class which is provided by IoT hub.
                //Message need payload(i.e input) but that payload shoud be in Array of bytes not a string. So we have to convert string into Array of bytes.
                //So lets first convert string into Array of bytes and then we will give that varibale as input of Message.
                var stringToArrayBytes = Encoding.ASCII.GetBytes("Hello from Band Agent");
                var message = new Message(stringToArrayBytes);
                
                //Its time to send a message to cloud.
                await device.SendEventAsync(message);
                Console.WriteLine("Message send to the cloud");

                //lets take break for 2 second after sending a message.
                Thread.Sleep(2000);
                //We can now send a simple message from our Device to Hub. 
                //Open Ternimal (lets say CMD1) : write => iothub -explorer monitor-events --login "Azure IoT Hub Connection String"
                //Now run the Globomatics.BandAgent Solution, We can see that from Device Application we are sending messages and in CMD1 we are receiving that means in IoT Hub.

                // There is one problem that in reallity we are not sending message. We are sending an Object. So lets create one class which has "Message" and "StatusCode as a property"
                //We have created Telemetry.cs 
            }
            Console.WriteLine("Press Key to exit..");
            Console.ReadKey();
        }
    }
}
