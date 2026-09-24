using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Request
{
    public class VacateBookingRequestDTO
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
    }
}
