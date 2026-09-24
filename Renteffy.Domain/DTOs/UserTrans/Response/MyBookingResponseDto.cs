using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Response
{
    public class MyBookingResponseDto
    {
        public int BookingId { get; set; }
        public string BookingCode { get; set; } = default!;
        public int PostId { get; set; }
        public string PgName { get; set; } = default!;
        public string? ApartmentName { get; set; }
        public string AreaName { get; set; } = default!;
        public string City { get; set; } = default!;
        public string? FloorNumber { get; set; }
        public string? RoomNumber { get; set; }
        public string? SharingType { get; set; }
        public int BedId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public decimal Price { get; set; }

        public string? FilePath { get; set; }

        public int Status { get; set; }

        public int PaymentStatus { get; set; }

        public string OwnerName { get; set; } = default!;

        public string Mobile { get; set; } = default!;
    }
}
