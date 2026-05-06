using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Shared.Security
{
    public class VerifyPaymentRequestDto
    {
        public int BookingId { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Signature { get; set; }
    }
}
