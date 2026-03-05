using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelTrack.DTOs
{
    internal class BookingStatusDto
    {
        public string ParcelId { get; set; }
        public bool ShipmentConfirmed { get; set; } = false;
        public string ShipmentCost { get; set; }
    }
}
