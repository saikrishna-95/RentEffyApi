using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Response
{
    public class CreateBookingPaymentVerificationResponseDTO
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public decimal Price { get; set; }
        public string? RazorpayOrderId { get; set; }
        public int Status { get; set; }
        public int PaymentStatus { get; set; }
    }
}
