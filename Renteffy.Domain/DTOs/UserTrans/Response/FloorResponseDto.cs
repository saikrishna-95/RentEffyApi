using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Response
{
    public class FloorResponseDto
    {
        [Key]
        public int FloorId { get; set; }
        public int FloorNumber { get; set; }
    }
}
