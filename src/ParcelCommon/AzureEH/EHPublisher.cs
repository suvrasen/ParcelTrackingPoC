using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using ParcelCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelCommon.AzureEH
{
    internal class EHPublisher : IEHPublisher
    {
        private string _connectionString = null;
        private string _hubName = null;

        public EHPublisher(string ConnectionString, string HubName) 
        {
            _connectionString = ConnectionString;
            _hubName = HubName;
        }
        async Task<bool> IEHPublisher.PublishSingleEvent(string EvtMsg)
        {
            try
            {
                await using (var _producerClient = new EventHubProducerClient(_connectionString, _hubName))
                {
                    EventData eventData = new EventData(UTF8Encoding.UTF8.GetBytes(EvtMsg));
                    await _producerClient.SendAsync(new[] { eventData });

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        Task<bool> IEHPublisher.PublishBatchEvent(string[] EvtMsg)
        {
            throw new NotImplementedException();
        }
    }
}
