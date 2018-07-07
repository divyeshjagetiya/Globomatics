using Globomatics.Common;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Globomatics.MessageProcessor
{
    //All processor has to be impliment IEventProcessor
    public class LoginEventProcessor : IEventProcessor
    {
        /// <summary>
        /// This method is called when our processor stop processing partition.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine("LoginEventProcessor closing, Partition: " + $"'{context.PartitionId}' , reason : '{reason}'.");
            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is called when our processor is attached to a new partition for a hub
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine("LoginEventProcessor opened, Processing Partition: " + $"'{context.PartitionId}'");
            return Task.CompletedTask;
        }

        /// <summary>
        /// This is called anytime when somethings goes wrong with our processor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine("LoginEventProcessor error, Partition: " + $"'{context.PartitionId}' , error : '{error}'.");
            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is called when data comes into our hub will be sent here for the processing 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            Console.WriteLine($"Batch of events received on Partition '{context.PartitionId}'.");
            //Now we need to process each event
            foreach (var eventData in messages )
            {
                //Each event has the payload that we want to process, Thats the payload sent by our Device
                var payload = Encoding.ASCII.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                //We can get the other imformation from the event from System Properties
                var deviceId = eventData.SystemProperties["iothub-connection-device-id"];
                Console.WriteLine($"Message received on Partition '{context.PartitionId}' ,"+ $" DeviceId: '{deviceId}'" + $" Payload: {payload}");

                var telemetry = JsonConvert.DeserializeObject<Telemetry>(payload);

                if(telemetry.Status == StatusType.Emergency)
                {
                    Console.WriteLine($"Guest requires emergency assistance! Device ID: {deviceId}");
                    SendFirstRespondersTo(telemetry.Latitute, telemetry.Longitude);
                }
            }

            // Now whenever we get data we have to give a checkpoint to that blob so that it should not process old messages.
            return context.CheckpointAsync();
        }

        private void SendFirstRespondersTo(decimal latitute, decimal longitude)
        {
            //In a real app , This is where we would send a command or notification!
            Console.WriteLine($"**First responders dispatched to ({latitute},{longitude})!");
        }
    }
}
