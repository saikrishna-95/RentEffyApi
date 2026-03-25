using Renteffy.Domain.DTOs.Owner.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.Owner.Response
{
    public class EditOwnerPostResponseDto
    {
        public int PostId { get; set; }
        public int OwnerId { get; set; }
        public int CategoryId { get; set; }
        public int PgTypeId { get; set; }

        public string PgName { get; set; }
        public string ApartmentName { get; set; }
        public string HouseNo { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string AreaName { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string Pincode { get; set; }
        public string Mobile { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public int TotalFloors { get; set; }
        public int TotalRooms { get; set; }
        public int Status { get; set; }

        public List<RoomPricingDto> RoomPricing { get; set; }

        public List<PostMediaDto> Media { get; set; }
        public List<AmenitiesDto> Amenities { get; set; }
        public List<StayingPeriodsPostDto> StayingPeriods { get; set; }
        public List<FoodPostDto> FoodPosts { get; set; }
    }
}
