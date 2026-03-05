using ParcelCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelCommon.Interfaces
{
    public interface ICDBManager
    {
        void LoadEventTypesMaster();
        void LoadShippingCostMaster();

        Task<bool> CreateNewParcel(Parcel parcel);
        Task<bool> CreateEvent(ParcelEvent newEvent);
        Task<Parcel> GetParcelDetail(string TrackingId);
        Task<ParcelEvent> GetParcelEvent(string TrackingId, string EventCode, string LocId);
        Task<List<ParcelEvent>> GetAllOrderedEventsByTrackingId(string TrackingId);
    }
}
