using System;
using System.Text;
using System.Collections.Generic;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Amqp;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Dynamic;
using ParcelCommon.Models;
using ParcelCommon.Interfaces;
using System.Drawing;
using ParcelTrack.Interfaces;

namespace ParcelTrack
{
    public class EventConsumer
    {
        ILogger<EventConsumer> _logger;
        IEventConsumerSvc _eventConsumerSvc;

        public EventConsumer(ILogger<EventConsumer> logger, IEventConsumerSvc eventConsumerSvc)
        {
            _logger = logger;
            _eventConsumerSvc = eventConsumerSvc;
        }

        [Function("EventReader")]
        public async Task Run([EventHubTrigger(eventHubName: "parcel_scan", Connection = "ConnectionDetails", ConsumerGroup = "group1")] EventData[] events)
        {

            
            foreach (EventData @event in events)
            {

                _logger.LogInformation("Event Body Deserialized: {body}", Encoding.UTF8.GetString(@event.Data));
                _logger.LogInformation("Event Body: {body}", @event.Body);
                _logger.LogInformation("Event Offset & Sequence {offset} {seq}", @event.OffsetString, @event.SequenceNumber);
                _logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);

                dynamic eventData = JsonSerializer.Deserialize<ExpandoObject>(Encoding.UTF8.GetString(@event.Data))!;
                var result = await _eventConsumerSvc.ProcessEventData(eventData);

                //Write a message to AppInsight with value of result
            }
        }
    }
}
