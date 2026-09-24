using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs
{
    public class AddPropertyRequestDto
    {
        public int UserId { get; set; }

        public int PropertyTypeID { get; set; }

        public string Mobile { get; set; } = default!;

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string PropertyName { get; set; } = default!;

        public string HouseNo { get; set; } = default!;

        public string Street { get; set; } = default!;

        public string Area { get; set; } = default!;

        public string City { get; set; } = default!;

        public string State { get; set; } = default!;

        public string PinCode { get; set; } = default!;

        public string? FullAddress { get; set; }

        public int? PropertyConfigID { get; set; }

        public int? Bedrooms { get; set; }

        public int? Bathrooms { get; set; }

        public int? Balconies { get; set; }

        public string? FloorDetails { get; set; }

        public int? RentalTypeID { get; set; }

        public int? PropertyFurnishID { get; set; }

        public decimal? MonthlyRentPrice { get; set; }

        public decimal? SecurityDepositAmount { get; set; }

        public int? MaintenanceID { get; set; }

        public DateTime? AvailableFromDate { get; set; }

        public int? PreferredTenantID { get; set; }

        public string? AboutHome { get; set; }

        public List<PropertyAmenitiesDto> Amenities { get; set; } = new();

        public List<PropertyRulesDto> Rules { get; set; } = new();

        public List<PropertyMediaDto> Media { get; set; } = new();
    }
}
