namespace Renteffy.Domain.DTOs.Owner.Request
{
    public class AddPostRequestDto
    {
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int PgTypeId { get; set; }
        public string PgName { get; set; } = default!;
        public string? ApartmentName { get; set; }
        public string Address { get; set; } = default!;
        public string Pincode { get; set; } = default!;
        public string Mobile { get; set; } = default!;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int TotalFloors { get; set; }
        public int TotalRooms { get; set; }
        public List<RoomPricingDto> RoomPricing { get; set; }
        public List<AmenitiesDto> Amenities { get; set; }

        public List<StayingPeriodsPostDto> StayingPeriods { get; set; }
        public List<FoodPostDto> FoodPosts { get; set; }
    }
}
