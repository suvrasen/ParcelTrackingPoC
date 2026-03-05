using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Cosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ParcelCommon.Models
{
    
    public class Parcel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ParcelId { get; set; }
        public string TrackingID { get; set; }
        public decimal ShippingCost { get; set; }
        public ParcelDimension ParcelDimension { get; set; }
        public string SenderName { get; set; }
        public string SenderPhone { get; set; }
        public string ReceiverName { get; set; }
        public string? ReceiverPhone { get; set; }
        public AddressDto SourceAddress { get; set; }
        public AddressDto DestinationAddess { get; set; }
        public bool ReceiverAlert { get; set; }
    }
    [Owned]
    public class ParcelDimension
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? Length { get; set; }
        public int? Weight { get; set; }
    }
    [Owned]
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
