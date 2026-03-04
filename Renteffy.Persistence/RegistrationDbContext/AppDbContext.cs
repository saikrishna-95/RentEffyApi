using Microsoft.EntityFrameworkCore;
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
    }
}
