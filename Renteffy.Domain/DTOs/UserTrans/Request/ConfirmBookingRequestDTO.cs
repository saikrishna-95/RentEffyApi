using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Request
{
    public class ConfirmBookingRequestDTO
    {
        public int BookingId { get; set; }
        public string TransactionId { get; set; }

        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Signature { get; set; }
    }
}
