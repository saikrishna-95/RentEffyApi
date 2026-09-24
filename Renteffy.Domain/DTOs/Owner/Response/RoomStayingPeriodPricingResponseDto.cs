using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.Owner.Response
{
    public class RoomStayingPeriodPricingResponseDto
    {
        public int RoomPeriodPricingId { get; set; }
        public int PostId { get; set; }
        public int FloorId { get; set; }
        public int? FloorNumber { get; set; }
        public int RoomId { get; set; }
        public int? RoomNumber { get; set; }
        public int StngPrdId { get; set; }
        public string? StayingPeriodName { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
