using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ParcelCommon.Interfaces;
using ParcelCommon.Models;
using ParcelCommon.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelCommon
{
    public class CDBManager : ICDBManager
    {
        ILogger<CDBManager> _logger;
        CDBContext _dbContext;

        public CDBManager(ILogger<CDBManager> logger,CDBContext CosmosDBContext) 
        { 
            _logger = logger;
            _dbContext = CosmosDBContext;
        }

        async Task<bool> ICDBManager.CreateNewParcel(Parcel parcel)
        {
            //_dbContext.Add<Parcel>(parcel);
            _dbContext.Parcels.Add(parcel);
            _ = await _dbContext.SaveChangesAsync();
           return true;
        }

        async Task<bool> ICDBManager.CreateEvent(ParcelEvent newEvent)
        {
            //_dbContext.Add<Parcel>(parcel);
            _dbContext.Events.Add(newEvent);
            _ = await _dbContext.SaveChangesAsync();
            return true;
        }

        async Task<Parcel> ICDBManager.GetParcelDetail(string TrackingId)
        {
            var parcel = await _dbContext.Parcels.FirstOrDefaultAsync(parcel => parcel.TrackingID == TrackingId);
            return parcel;
        }

        async Task<ParcelEvent> ICDBManager.GetParcelEvent(string TrackingId, string EventCode, string LocId)
        {
            var parcelEvent = await _dbContext.Events.FirstOrDefaultAsync(evt => evt.TrackingId == TrackingId && evt.EventType == EventCode);
            return parcelEvent;
        }

        async Task<List<ParcelEvent>> ICDBManager.GetAllOrderedEventsByTrackingId(string TrackingId)
        {
            var parcelEvents = await _dbContext.Events
                .Where(q => q.TrackingId == TrackingId)
                .OrderByDescending(o => o.TimeStamp)
                .ToListAsync<ParcelEvent>();
            return parcelEvents;
        }

        private DateTime ConverToDateTime(string ts)
        {
            long lStr = long.Parse(ts);
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(lStr);
            return dateTimeOffset.DateTime;
        }

        void ICDBManager.LoadEventTypesMaster()
        {
            _logger.LogInformation("Load all Event types from database, store it for faster performance.");
        }

        void ICDBManager.LoadShippingCostMaster()
        {
            _logger.LogInformation("Load Shipping costs from database and store it for faster performance.");
        }


    }
}
