using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelTrack.Interfaces
{
    public interface IEventConsumerSvc
    {
        Task<bool> ProcessEventData(dynamic eventData);
    }
}
