using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Response
{
    public class CreateBookingResponseDTO
    {
        public int BookingId { get; set; }
        public decimal Price { get; set; }
    }
}
