using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ParcelCreate.DTOs
{
    [ExcludeFromCodeCoverage]
    public class BookingRequestDto
    {
        [Required, NotNull]
        public string ActorId { get; set; }

        [Required, NotNull]
        public string ActorType { get; set; }
        public string EventType { get; set; }
        public string LocationId { get; set; }
        public string HubType { get; set; }

        [Required, NotNull]
        public PersonDto SenderData { get; set; }

        [Required, NotNull]
        public PersonDto ReceiverData { get; set; }

        [Required, NotNull]
        public ParcelDto ParcelData { get; set; }

        public bool ReceiverNotification { get; set; } = false;

    }
}
