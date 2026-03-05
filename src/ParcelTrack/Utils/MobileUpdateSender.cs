using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParcelTrack.Interfaces;

namespace ParcelTrack.Utils
{
    internal class MobileUpdateSender : IAlertSender
    {
        //Logic for sending status to mobile phone no. will be implemented here.
        //It is a placeholder
        ILogger<MobileUpdateSender> _logger;

        public MobileUpdateSender(ILogger<MobileUpdateSender> logger) 
        {
            _logger = logger;
        }

        async Task IAlertSender.SendMobileUpdate(string MobileNo)
        {
            _logger.LogInformation($"Alert sent to mobile no. {MobileNo}");
        }
    }
}
