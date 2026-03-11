using Renteffy.Domain.DTOs.Owner;
using Renteffy.Domain.DTOs.Owner.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.User.Response
{
    public class PublicPostResponseDto
    {
        public int PostId { get; set; }
        public int PgTypeId { get; set; }
        public string PgName { get; set; } = default!;
        public string? ApartmentName { get; set; }
        public string Address { get; set; } = default!;
        public string Pincode { get; set; } = default!;
        public string Mobile { get; set; } = default!;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public List<PostMediaDto> Media { get; set; } = [];
        public List<RoomPricingDto> Pricing { get; set; } = [];

        public List<AmenitiesDto> Amenities { get; set; } = [];
        public List<StayingPeriodsPostDto> stayingPeriods { get; set; } = [];
        public List<FoodPostDto> foodPosts { get; set; } = [];
    }
}
