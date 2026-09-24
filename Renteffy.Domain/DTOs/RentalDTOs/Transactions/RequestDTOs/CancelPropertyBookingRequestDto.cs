using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs
{
    public class CancelPropertyBookingRequestDto
    {
        public int UserID { get; set; }
        public int PropertyBookingID { get; set; }
        public string? Reason { get; set; }
    }
}
