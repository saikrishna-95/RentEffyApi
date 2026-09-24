using Microsoft.EntityFrameworkCore;
using Renteffy.Domain.DTOs.RentalDTOs.Masters.Response;
using Renteffy.Domain.DTOs.UserTrans.Response;
using Renteffy.Domain.Entities.Registration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Persistence.RegistrationDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Users> Users { get; set; }
        public DbSet<AmenitesResponseDto> M_Amenities_MT { get; set; }
        public DbSet<BedTypeResponseDto> M_BedTypes_MT { get; set; }
        public DbSet<CategoryResponseDto> M_Categories_MT { get; set; }
        public DbSet<FloorResponseDto> M_Floors_MT { get; set; }
        public DbSet<FoodResponseDto> M_FOOD_MT { get; set; }
        public DbSet<PgTypeResponseDto> M_PGTYPE_MT { get; set; }
        public DbSet<RoomResponseDto> M_Rooms_MT { get; set; }
        public DbSet<StayingPeriodResponseDto> M_STAYING_PERIOD_MT { get; set; }

        public DbSet<PropertyTypeResponseDto> M_Property_Type_MT { get; set; }

        public DbSet<PropertyConfigurationResponseDto> M_Property_Configuration_MT { get; set; }

        public DbSet<PropertyRoomsResponseDto> M_Property_Rooms_MT { get; set; }

        public DbSet<FurnishingResponseDto> M_Furnishing_MT { get; set; }

        public DbSet<MaintenanceResponseDto> M_Maintenance_MT { get; set; }

        public DbSet<PropertyAmenitesResponseDto> M_Property_Amenities_MT { get; set; }

        public DbSet<PreferredTenantsResponseDto> M_Preferred_Tenants_MT { get; set; }

        public DbSet<PropertyRulesResponseDto> M_Property_Rules_MT { get; set; }

        public DbSet<RentalTypeResponseDto> M_RentalType_MT { get; set; }

        public DbSet<PropertyPhotosVideosResponseDto> M_Property_PhotosVideos_MT { get; set; }
    }
}
