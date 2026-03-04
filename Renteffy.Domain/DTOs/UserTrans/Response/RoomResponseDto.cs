using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Response
{
    public class RoomResponseDto
    {
        [Key]
        public int RoomId { get; set; }
        public int RoomNumber { get; set; }
    }
}
