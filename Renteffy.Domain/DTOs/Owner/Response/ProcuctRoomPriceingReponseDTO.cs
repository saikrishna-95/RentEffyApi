using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.Owner.Response
{
    public class ProcuctRoomPriceingReponseDTO
    {
        public int PostId { get; set; }
        public int FloorId { get; set; }
        public int FloorNumber { get; set; }
        public int RoomId { get; set; }

        public int RoomNumber { get; set; }
        public int BedTypeId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
