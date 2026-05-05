using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.Owner
{
    public class FoodPostDto
    {
        public int PostId { get; set; }
        public int FoodId { get; set; }
        public string? Name { get; set; }
    }
}
