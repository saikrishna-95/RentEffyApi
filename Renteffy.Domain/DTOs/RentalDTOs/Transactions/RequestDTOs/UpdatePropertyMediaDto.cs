using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs
{
    public class UpdatePropertyMediaDto
    {
        public int PropertyPostMediaID { get; set; }
        public int PropertyPostID { get; set; }
        public int PropertyMediaTypeID { get; set; }
        public string? MediaType { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? ContentType { get; set; }
        public int UpdatedBy { get; set; }
    }
}
