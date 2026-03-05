using System.Diagnostics.CodeAnalysis;

namespace ParcelCreate.DTOs
{
    [ExcludeFromCodeCoverage]
    public class ParcelDto
    {
        public string TrackingID { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? Length { get; set; }
        public int? Weight { get; set; }
        public AddressDto SourceAddress { get; set; }
        public AddressDto DestinationAddress { get; set; }
        //public bool? ReceiverNotification { get; set; } = false;
    }
}
