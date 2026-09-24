using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Renteffy.Domain.DTOs.RentalDTOs.Masters.Response
{
    public class PreferredTenantsResponseDto
    {
        [Key]
        public int PreferredTenantID { get; set; }
        public string Name { get; set; }

        public int Status { get; set; }
    }
}
