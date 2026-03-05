using System.Diagnostics.CodeAnalysis;

namespace ParcelCreate.DTOs
{
    [ExcludeFromCodeCoverage]
    public class AddressDto
    {
        public string Address1 { get; set; }
        public string? Address2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public int PostalCode { get; set; }
        public string Country { get; set; }
    }
}
