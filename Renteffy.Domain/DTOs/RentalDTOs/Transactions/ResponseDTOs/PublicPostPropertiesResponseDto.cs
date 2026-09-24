using Renteffy.Domain.DTOs.RentalDTOs.Masters.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.RentalDTOs.Transactions.ResponseDTOs
{
    public class PublicPostPropertiesResponseDto
    {
        public int PropertyID { get; set; }

        public int OwnerID { get; set; }

        public int PropertyTypeID { get; set; }

        public string PropertyTypeName { get; set; } = default!;

        public string Mobile { get; set; } = default!;

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string PropertyName { get; set; } = default!;

        public string? HouseNo { get; set; }

        public string? Street { get; set; }

        public string? Area { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? PinCode { get; set; }

        public string? FullAddress { get; set; }

        public int? PropertyConfigID { get; set; }

        public string? PropertyConfigName { get; set; }

        public int? Bedrooms { get; set; }

        public int? Bathrooms { get; set; }

        public int? Balconies { get; set; }

        public string? FloorDetails { get; set; }

        public int? RentalTypeID { get; set; }

        public string? RentalTypeName { get; set; }

        public int? PropertyFurnishID { get; set; }

        public string? PropertyFurnishName { get; set; }

        public decimal? MonthlyRentPrice { get; set; }

        public decimal? SecurityDepositAmount { get; set; }

        public int? MaintenanceID { get; set; }

        public string? MaintenanceName { get; set; }

        public DateTime? AvailableFromDate { get; set; }

        public int? PreferredTenantID { get; set; }

        public string? PreferredTenantName { get; set; }

        public string? AboutHome { get; set; }

        public List<PropertyAmenitiesResponseDto> Amenities { get; set; } = [];

        public List<PropertyRulesResponseDto> Rules { get; set; } = [];

        public List<PropertyMediaResponseDto> Media { get; set; } = [];
    }
}
