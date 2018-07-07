/* 
 * Divyesh Jagetiya
 * Date: 06-July-2018
 */
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Threading.Tasks;

namespace Globomatics.MessageProcessor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Lets Connect with Hub. Goto Azure-> IOT hub -> Endpoints
            var hubName = "iothub-ehub-diotdemo-526220-62a863be8d";
            var iotHubConnectionString = "Endpoint=sb://ihsuprodmares009dednamespace.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=D4YZFdyEL1DaDoaMOajY5yjZwNsIKzdNHltrXEAgEVA=";

            //But our processor need somewhere to track its progess to processing messages. For that it will store checkpoint and azure blob storage.
            //For storage : Azure -> iot Storage-> Blob -> Add Container-> Name as "message-processor-host" Its take care of container.
            //Now let gets storage connection string that will under accesskey.
            var storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=iotdemostorage01;AccountKey=axLgwLqvqmh0cfsbfMXjjS5UINy8rvPB9MrNeJas9Vkd67yyF8JyvdEPTTY5nhyzu2xBiMvwWA2hLwLgeJ/a3w==;EndpointSuffix=core.windows.net";
            var storageContainerName = "message-processor-host";
            var consumerGroupName = PartitionReceiver.DefaultConsumerGroupName;

            //Now we have to create host for Event processor using all the above data as an input.
            var processor = new EventProcessorHost(hubName, consumerGroupName, iotHubConnectionString, storageConnectionString, storageContainerName);

            //But we didn't register out host yet. So we have do that as well.
            await processor.RegisterEventProcessorAsync<LoginEventProcessor>();

            Console.WriteLine("Event processor started, press enter key to exit...");
            Console.ReadKey();

            await processor.UnregisterEventProcessorAsync();
        }
    }
}
