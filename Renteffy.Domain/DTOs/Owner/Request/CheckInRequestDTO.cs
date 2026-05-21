using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.Owner.Request
{
    public class CheckInRequestDTO
    {
        public int BookingId { get; set; }
        public int OwnerId { get; set; }
    }
}
