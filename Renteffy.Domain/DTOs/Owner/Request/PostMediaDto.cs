using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.Owner.Request
{
    public class PostMediaDto
    {
        public int PostId { get; set; }
        public string MediaType { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public string? FileUrl { get; set; } = default; 
    }
}
