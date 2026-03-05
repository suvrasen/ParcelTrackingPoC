using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelTrack.Interfaces
{
    public interface IValidateScan
    {
        Task<bool> ValidateBookingAsync(string TrackingID);
        Task<bool> ValidateScanEventAsync(string TrackingID, string EventTypeCode, string LocId);
        decimal CalculateShippingCost(int Width, int Height, int Length, int Weight);
    }
}
