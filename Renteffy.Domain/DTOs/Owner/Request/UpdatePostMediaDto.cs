using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.Owner.Request
{
    public class UpdatePostMediaDto
    {
        public int MediaId { get; set; }
        public int PostId { get; set; }
        public string MediaType { get; set; }
        public string FileName { get; set; } // PublicId
        public string FilePath { get; set; }
        public string ContentType { get; set; }
    }
}
