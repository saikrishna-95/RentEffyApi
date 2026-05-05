using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.Owner
{
    public class StayingPeriodsPostDto
    {
        public int PostId { get; set; }
        public int StngPrdId { get; set; }
        public string? Name { get; set; }
    }
}
