using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.Owner.Request
{
    public class UpdatePostRequestDTO
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int PgTypeId { get; set; }

        public string PgName { get; set; }
        public string ApartmentName { get; set; }

        public string HouseNo { get; set; }
        public string Street { get; set; }
        public string AreaName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }

        public string Mobile { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public int TotalFloors { get; set; }
        public int TotalRooms { get; set; }

        public List<RoomPricingDto> RoomPricing { get; set; }
        public List<AmenitiesDto> Amenities { get; set; }
        public List<StayingPeriodsPostDto> StayingPeriods { get; set; }
        public List<FoodPostDto> FoodPosts { get; set; }
        public List<int> DeleteMediaIds { get; set; } = new();
    }
}
