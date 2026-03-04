using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Response
{
    public class FoodResponseDto
    {
        [Key]
        public int FoodId { get; set; }
        public string Name { get; set; }
    }
}
