using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelCommon.Models
{
    public class ParcelEvent
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TrackingId { get; set; }
        public string ActorId { get; set; }
        public string ActorType { get; set; }
        public string LocationId { get; set; }
        public string HubType { get; set; }
        public string EventType { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
