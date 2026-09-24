using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.RentalDTOs.Transactions.ResponseDTOs
{
    public class CreatePropertyBookingResponseDto
    {
        public int PropertyBookingID { get; set; }
        public decimal TotalAmount { get; set; }
        public string RazorpayOrderID { get; set; } = default!;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "INR";
        public string RazorpayKey { get; set; } = default!;
    }
}
