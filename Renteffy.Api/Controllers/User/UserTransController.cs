using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Renteffy.Application.Interfaces.Owner;
using Renteffy.Application.Interfaces.User;
using Renteffy.Domain.DTOs.UserTrans.Response;
using Renteffy.Persistence.RegistrationDbContext;

namespace Renteffy.Api.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTransController : ControllerBase
    {
        private readonly IGetOwnerPostsApplication _readApp;
        private readonly IMemoryCache _cache;
        private readonly AppDbContext _context;
        public UserTransController(IGetOwnerPostsApplication readApp, AppDbContext context, IMemoryCache cache)
        {
            _readApp = readApp;
            _context = context;
            _cache = cache;
        }

        [HttpGet("GetAllMaters")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllMasterData()
        {
            var data = new MasterDataResponseDTO
            {
                Amenities = await _context.M_Amenities_MT
                    .AsNoTracking()
                    .Select(x => new AmenitesResponseDto
                    {
                        AmenityId = x.AmenityId,
                        Name = x.Name
                    })
                    .ToListAsync(),

                BedTypes = await _context.M_BedTypes_MT
                    .AsNoTracking()
                    .Select(x => new BedTypeResponseDto
                    {
                        BedTypeId = x.BedTypeId,
                        Name = x.Name
                    })
                    .ToListAsync(),

                Categories = await _context.M_Categories_MT
                    .AsNoTracking()
                    .Select(x => new CategoryResponseDto
                    {
                        CategoryId = x.CategoryId,
                        Name = x.Name
                    })
                    .ToListAsync(),

                Floors = await _context.M_Floors_MT
                    .AsNoTracking()
                    .Select(x => new FloorResponseDto
                    {
                        FloorId = x.FloorId,
                        FloorNumber = x.FloorNumber
                    })
                    .ToListAsync(),

                FoodTypes = await _context.M_FOOD_MT
                    .AsNoTracking()
                    .Select(x => new FoodResponseDto
                    {
                        FoodId = x.FoodId,
                        Name = x.Name
                    })
                    .ToListAsync(),

                PgTypes = await _context.M_PGTYPE_MT
                    .AsNoTracking()
                    .Select(x => new PgTypeResponseDto
                    {
                        PgTypeId = x.PgTypeId,
                        Name = x.Name
                    })
                    .ToListAsync(),

                Rooms = await _context.M_Rooms_MT
                    .AsNoTracking()
                    .Select(x => new RoomResponseDto
                    {
                        RoomId = x.RoomId,
                        RoomNumber = x.RoomNumber
                    })
                    .ToListAsync(),

                StayingPeriods = await _context.M_STAYING_PERIOD_MT
                    .AsNoTracking()
                    .Select(x => new StayingPeriodResponseDto
                    {
                        StngPrdId = x.StngPrdId,
                        Name = x.Name
                    })
                    .ToListAsync()
            };

            return Ok(data);
        }

        [AllowAnonymous]
        [HttpGet("OwnerPosts")]
        public async Task<IActionResult> GetOwnerPosts()
        {
            var posts = await _readApp.GetPublicPostsAsync();
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            foreach (var post in posts)
            {
                foreach (var media in post.Media)
                {
                    media.FileUrl = $"{baseUrl}{media.FilePath}{media.FileName}";
                }
            }
            return Ok(posts);
        }
    }
}
