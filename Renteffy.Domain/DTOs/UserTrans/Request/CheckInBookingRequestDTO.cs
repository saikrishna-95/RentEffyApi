using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Request
{
    public class CheckInBookingRequestDTO
    {
        public int BookingId { get; set; }
        public int OwnerId { get; set; }
    }
}
