using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Request
{
    public class BookingPaymentDetailsDto
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public string? RazorpayPaymentId { get; set; }
        public int PaymentStatus { get; set; }
        public int Status { get; set; }
    }
}
