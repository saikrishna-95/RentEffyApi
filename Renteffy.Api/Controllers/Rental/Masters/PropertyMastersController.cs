using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Renteffy.Application.Interfaces.User;
using Renteffy.Domain.DTOs.RentalDTOs.Masters.Response;
using Renteffy.Domain.DTOs.UserTrans.Response;
using Renteffy.Persistence.RegistrationDbContext;

namespace Renteffy.Api.Controllers.Rental.Masters
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyMastersController : ControllerBase
    {
        private readonly IGetOwnerPostsApplication _readApp;
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _context;
        public PropertyMastersController(IGetOwnerPostsApplication readApp, AppDbContext context, IMemoryCache cache)
        {
            _readApp = readApp;
            _context = context;
            _cache = cache;
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpGet("GetAllPropertyMasters")]
        public async Task<IActionResult> GetAllPropertyMasters()
        {
            var data = new AllPropertyMastersDataDTO
            {
                PropertyTypes = await _context.M_Property_Type_MT
                                .AsNoTracking()
                                .Where(x => x.Status == 1)
                                .OrderBy(x => x.PropertyTypeID)
                                .Select(x => new PropertyTypeResponseDto
                                {
                                    PropertyTypeID = x.PropertyTypeID,
                                    Name = x.Name
                                }).ToListAsync(),

                PropertyConfigurations = await _context.M_Property_Configuration_MT
                                        .AsNoTracking()
                                        .Where(x => x.Status == 1)
                                        .OrderBy(x => x.PropertyConfigID)
                                        .Select(x => new PropertyConfigurationResponseDto
                                        {
                                            PropertyConfigID = x.PropertyConfigID,
                                            Name = x.Name
                                        }).ToListAsync(),

                PropertyRooms = await _context.M_Property_Rooms_MT
                                .AsNoTracking()
                                .Where(x => x.Status == 1)
                                .OrderBy(x => x.PropertyRoomID)
                                .Select(x => new PropertyRoomsResponseDto
                                {
                                    PropertyRoomID = x.PropertyRoomID,
                                    Name = x.Name
                                }).ToListAsync(),

                Furnishing = await _context.M_Furnishing_MT
                            .AsNoTracking()
                            .Where(x => x.Status == 1)
                            .OrderBy(x => x.PropertyFurnishID)
                            .Select(x => new FurnishingResponseDto
                            {
                                PropertyFurnishID = x.PropertyFurnishID,
                                Name = x.Name
                            }).ToListAsync(),

                Maintenance = await _context.M_Maintenance_MT
                                .AsNoTracking()
                                .Where(x => x.Status == 1)
                                .OrderBy(x => x.MaintenanceID)
                                .Select(x => new MaintenanceResponseDto
                                {
                                    MaintenanceID = x.MaintenanceID,
                                    Name = x.Name
                                }).ToListAsync(),

                PropertyAmenities = await _context.M_Property_Amenities_MT
                                    .AsNoTracking()
                                    .Where(x => x.Status == 1)
                                    .OrderBy(x => x.PropertyAmenityID)
                                    .Select(x => new PropertyAmenitesResponseDto
                                    {
                                        PropertyAmenityID = x.PropertyAmenityID,
                                        Name = x.Name
                                    }).ToListAsync(),

                PreferredTenants = await _context.M_Preferred_Tenants_MT
                                    .AsNoTracking()
                                    .Where(x => x.Status == 1)
                                    .OrderBy(x => x.PreferredTenantID)
                                    .Select(x => new PreferredTenantsResponseDto
                                    {
                                        PreferredTenantID = x.PreferredTenantID,
                                        Name = x.Name
                                    }).ToListAsync(),

                PropertyRules = await _context.M_Property_Rules_MT
                                .AsNoTracking()
                                .Where(x => x.Status == 1)
                                .OrderBy(x => x.RuleID)
                                .Select(x => new PropertyRulesResponseDto
                                {
                                    RuleID = x.RuleID,
                                    Name = x.Name
                                }).ToListAsync(),

                RentalTypes = await _context.M_RentalType_MT
                                .AsNoTracking()
                                .Where(x => x.Status == 1)
                                .OrderBy(x => x.RentalTypeID)
                                .Select(x => new RentalTypeResponseDto
                                {
                                    RentalTypeID = x.RentalTypeID,
                                    Name = x.Name
                                }).ToListAsync(),

                PropertyPhotosVideos = await _context.M_Property_PhotosVideos_MT
                                        .AsNoTracking()
                                        .Where(x => x.Status == 1)
                                        .OrderBy(x => x.PropertyMediaTypeID)
                                        .Select(x => new PropertyPhotosVideosResponseDto
                                        {
                                            PropertyMediaTypeID = x.PropertyMediaTypeID,
                                            Name = x.Name
                                        }).ToListAsync()
            };

            return Ok(data);
        }
    }
}
