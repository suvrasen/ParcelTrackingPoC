using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ParcelCreate.DTOs
{
    public class ParcelScanRequestDto
    {
        public string TrackingId { get; set; }
        public string ActorId { get; set; }
        public string ActorType { get; set; }
        public string LocationId { get; set; }
        public string HubType { get; set; }
        public string EventType { get; set; }

    }
}
