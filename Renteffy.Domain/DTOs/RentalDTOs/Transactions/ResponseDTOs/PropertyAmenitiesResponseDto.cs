using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.RentalDTOs.Transactions.ResponseDTOs
{
    public class PropertyAmenitiesResponseDto
    {
        public int PropertyPostID { get; set; }
        public int PropertyAmenityID { get; set; }
        public string Name { get; set; } = default!;
    }
}
