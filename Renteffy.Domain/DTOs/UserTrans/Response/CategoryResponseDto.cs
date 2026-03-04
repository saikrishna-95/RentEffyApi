using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Response
{
    public class CategoryResponseDto
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}
