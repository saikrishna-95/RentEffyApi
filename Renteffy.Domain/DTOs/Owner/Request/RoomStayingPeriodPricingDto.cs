using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.Owner.Request
{
    public class RoomStayingPeriodPricingDto
    {
        public int FloorId { get; set; }
        public int RoomId { get; set; }
        public int StngPrdId { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
