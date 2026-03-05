using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelCommon.Interfaces
{
    public interface IEHManager
    {
        Task<bool> PublishEvent(string EventMsg);
        Task<bool> PublishBatchEvent(string[] EventMsg);
    }
}
