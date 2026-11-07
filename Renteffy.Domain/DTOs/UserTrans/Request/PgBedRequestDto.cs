using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Request
{
    public class PgBedRequestDto
    {      
        public int FloorId { get; set; }
        public int RoomId { get; set; }
        public int BedTypeId { get; set; }
        public int BedId { get; set; }
        public string BedName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
