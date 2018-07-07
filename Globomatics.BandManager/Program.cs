using Microsoft.Azure.Devices;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Globomatics.BandManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //For connecting to Hub we need connection string of Service. iotHUb -> Shared Access -> Service
            //This string will allow to send and recevie which is on the cloud side
            var serviceConnetionSting = "HostName=DIoTDemo.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=aZ+VKFOMZaP3DoKMdakOvQWYmK7bHDE1YJZAMXxtvPw=";
            // Connectionstring will used for new service client 
            var serviceClient = ServiceClient.CreateFromConnectionString(serviceConnetionSting);

            //Now we have a client and we can use it

            while(true)
            {
                Console.WriteLine("Which device do you wish to send a message to? ");
                Console.Write("> ");
                var deviceId = Console.ReadLine();

                await SendCloudToDeviceMessage(serviceClient, deviceId);
            }
        }

        private static async Task SendCloudToDeviceMessage(ServiceClient serviceClient, string deviceId)
        {
            Console.WriteLine("What message payload do you want to send? ");
            Console.Write("> ");
            var payload = Console.ReadLine();

            var commandMessage = new Message(Encoding.ASCII.GetBytes(payload));
            //To Send a message to our target device we will use sendAsync method
            await serviceClient.SendAsync(deviceId, commandMessage);
        }
    }
}
