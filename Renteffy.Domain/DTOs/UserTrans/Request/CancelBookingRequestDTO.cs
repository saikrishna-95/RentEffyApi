using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Request
{
    public class CancelBookingRequestDTO
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
    }
}
