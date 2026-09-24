using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs
{
    public class ConfirmPropertyBookingRequestDto
    {
        public int UserID { get; set; }
        public int PropertyBookingID { get; set; }
        public string RazorpayOrderID { get; set; } = default!;
        public string RazorpayPaymentID { get; set; } = default!;
        public string RazorpaySignature { get; set; } = default!;
    }
}
