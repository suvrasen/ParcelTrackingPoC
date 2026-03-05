using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelCommon.Interfaces
{
    internal interface IEHPublisher
    {
        Task<bool> PublishSingleEvent(string EvtMsg);
        Task<bool> PublishBatchEvent(string[] EvtMsg);
    }
}
