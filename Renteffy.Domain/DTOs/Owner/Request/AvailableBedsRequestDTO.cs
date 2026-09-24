using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.Owner.Request
{
    public class AvailableBedsRequestDTO
    {
        public int PostId { get; set; }
        public int FloorId { get; set; }
        public int RoomId { get; set; }
        public int BedTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
