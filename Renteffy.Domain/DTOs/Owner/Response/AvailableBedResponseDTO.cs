using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.Owner.Response
{
    public class AvailableBedResponseDTO
    {
        public int BedId { get; set; }
        public int PostId { get; set; }
        public int FloorId { get; set; }
        public int RoomId { get; set; }
        public int BedTypeId { get; set; }
        public string BedName { get; set; }
        public bool IsAvailable { get; set; }
    }
}
