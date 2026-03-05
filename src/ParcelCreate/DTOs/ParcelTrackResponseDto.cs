namespace ParcelCreate.DTOs
{
    public class ParcelTrackResponseDto
    {
        public string TrackingId { get; set; }
        public string Status { get; set; }
        public string SourceCity { get; set; }
        public string SourceZipcode { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationZipcode { get; set; }
        public List<ParcelStatusTrackDto> TrackingDetails { get; set; }
    }

    public class ParcelStatusTrackDto
    {
        public string StatusDate { get; set; }
        public string StatusName { get; set; }
        public string StatusLoc { get; set; }
    }
}
