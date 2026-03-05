namespace ParcelCreate.DTOs
{
    public class BookingResponseDto
    {
        public string ParcelId { get; set; }
        public bool ShipmentConfirmed { get; set; }
        public float ShipmentCost { get; set; }
    }
}
