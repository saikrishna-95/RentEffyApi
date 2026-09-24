using Renteffy.Domain.DTOs.UserTrans.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.RentalDTOs.Masters.Response
{
    public class AllPropertyMastersDataDTO
    {
        public List<PropertyTypeResponseDto> PropertyTypes { get; set; }

        public List<PropertyConfigurationResponseDto> PropertyConfigurations { get; set; }

        public List<PropertyRoomsResponseDto> PropertyRooms { get; set; }

        public List<FurnishingResponseDto> Furnishing { get; set; }

        public List<MaintenanceResponseDto> Maintenance { get; set; }

        public List<PropertyAmenitesResponseDto> PropertyAmenities { get; set; }

        public List<PreferredTenantsResponseDto> PreferredTenants { get; set; }

        public List<PropertyRulesResponseDto> PropertyRules { get; set; }

        public List<RentalTypeResponseDto> RentalTypes { get; set; }

        public List<PropertyPhotosVideosResponseDto> PropertyPhotosVideos { get; set; }
    }
}
