using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.RentalDTOs.Transactions.ResponseDTOs
{
    public class PropertyRulesResponseDto
    {
        public int PropertyPostID { get; set; }
        public int RuleID { get; set; }
        public string Name { get; set; } = default!;
    }
}
