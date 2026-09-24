using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs
{
    public class CreatePropertyBookingRequestDto
    {
        public int PropertyID { get; set; }
        public int UserID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
