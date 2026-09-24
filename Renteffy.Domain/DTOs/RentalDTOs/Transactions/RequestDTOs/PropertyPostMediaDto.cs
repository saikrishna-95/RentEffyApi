using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs
{
    public class PropertyPostMediaDto
    {
        public int PropertyPostID { get; set; }
        public int PropertyMediaTypeID { get; set; }
        public string MediaType { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public string? FileUrl { get; set; } = default;
        public int CreatedBy { get; set; } = default;
    }
}
