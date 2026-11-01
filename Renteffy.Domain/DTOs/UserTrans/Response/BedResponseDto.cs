using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Response
{
    public class BedResponseDto
    {
        [Key]
        public int BedId { get; set; }
        public string BedName { get; set; } = string.Empty;
        public int BedTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}
