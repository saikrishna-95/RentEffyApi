using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Request
{
    public class CreateBookingRequestDTO
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public int FloorId { get; set; }
        public int RoomId { get; set; }
        public int BedTypeId { get; set; }
        public int StngPrdId { get; set; }
        public decimal Price { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }
}
