using ParcelCommon.Interfaces;
using ParcelCommon.AzureEH;
using Microsoft.Extensions.Options;
using ParcelCommon.Utilities;
using Azure.Messaging.EventHubs.Producer;

namespace ParcelCommon
{
    public class EHManager : IEHManager
    {
        IEHPublisher _eHPublisher;
        IEHSubscriber _eHSubscriber;
        EHSettings _ehSettings;

        public EHManager(IOptions<EHSettings> EHSetting)
        {
            _ehSettings = EHSetting.Value;
            _eHPublisher = new EHPublisher(_ehSettings.ConnectionString, _ehSettings.EHName);
        }
        async Task<bool> IEHManager.PublishBatchEvent(string[] EventMsg)
        {
            throw new NotImplementedException();
        }

        async Task<bool> IEHManager.PublishEvent(string EventMsg)
        {
            try
            {
                _ = await _eHPublisher.PublishSingleEvent(EventMsg);
                return true;
            }
            catch (Exception ex)
            {

            }

            return false;
        }
    }
}
