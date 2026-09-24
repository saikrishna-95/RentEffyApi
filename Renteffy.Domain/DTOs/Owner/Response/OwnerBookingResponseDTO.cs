using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.Owner.Response
{
    public class OwnerBookingResponseDTO
    {
        public int BookingId { get; set; }
        public string BookingCode { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public int PostId { get; set; }
        public string PgName { get; set; }
        public int FloorId { get; set; }
        public int RoomId { get; set; }
        public int BedId { get; set; }
        public decimal Price { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Status { get; set; }
        public int PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
