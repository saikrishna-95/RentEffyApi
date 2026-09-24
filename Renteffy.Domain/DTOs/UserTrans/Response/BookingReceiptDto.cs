using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Response
{
    public class BookingReceiptDto
    {
        public int BookingId { get; set; }
        public string BookingCode { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PgName { get; set; }
        public decimal Amount { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
