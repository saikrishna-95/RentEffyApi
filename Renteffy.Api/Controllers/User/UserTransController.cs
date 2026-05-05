using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Renteffy.Application.Interfaces.Owner;
using Renteffy.Application.Interfaces.User;
using Renteffy.Domain.DTOs.UserTrans.Response;
using Renteffy.Persistence.RegistrationDbContext;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

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

        [AllowAnonymous]
        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var data = new CategoriesMatersResponseDto
            {
                Categories = await _context.M_Categories_MT
                    .AsNoTracking()
                    .Select(x => new CategoryResponseDto
                    {
                        CategoryId = x.CategoryId,
                        Name = x.Name
                    })
                    .ToListAsync(),
            };
            return Ok(data);
        }

        [Authorize]
        [HttpGet("GetAllMasters")]
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
        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetOwnerPosts()
        {
            var posts = await _readApp.GetPublicPostsAsync();
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            foreach (var post in posts)
            {
                foreach (var media in post.Media)
                {
                    //media.FileUrl = $"{baseUrl}{media.FilePath}";
                    media.FileUrl = $"{media.FilePath}";
                }
            }
            return Ok(posts);
        }

        [AllowAnonymous]
        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetProductsAsync(int postid)
        {
            var posts = await _readApp.GetProductsAsync(postid);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            foreach (var post in posts)
            {
                foreach (var media in post.Media)
                {
                    //media.FileUrl = $"{baseUrl}{media.FilePath}";
                    media.FileUrl = $"{media.FilePath}";
                }
            }
            return Ok(posts);
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpPost("like")]
        public async Task<IActionResult> LikePost(int postId, int userId)
        {
            var result = await _readApp.LikePostAsync(postId, userId);
            return Ok(result);
        }

        [HttpGet("likescount")]
        public async Task<IActionResult> GetLikesCount(int postId, int userId)
        {
            var data = await _readApp.GetLikesCountAsync(postId, userId);
            return Ok(data);
        }

        [Authorize]
        [HttpPost("favorite")]
        public async Task<IActionResult> FavoritePost(int postId, int userId)
        {
            var result = await _readApp.FavoritePostAsync(postId, userId);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("favoritescount")]
        public async Task<IActionResult> GetFavoritesCount(int postId, int userId)
        {
            var data = await _readApp.GetFavoritesCountAsync(postId, userId);
            return Ok(data);
        }

        [Authorize]
        [HttpPost("comment")]
        public async Task<IActionResult> AddComment(int postId, int userId, string comment)
        {
            var result = await _readApp.AddCommentAsync(postId, userId, comment);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("comments/{postId}")]
        public async Task<IActionResult> GetComments(int postId)
        {
            var result = await _readApp.GetCommentsAsync(postId);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("request")]
        public async Task<IActionResult> SendRequest(int postId, int userId, string message)
        {
            var result = await _readApp.SendRequestAsync(postId, userId, message);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("owner/requests/{ownerId}")]
        public async Task<IActionResult> GetRequests(int ownerId)
        {
            var result = await _readApp.GetOwnerRequestsAsync(ownerId);
            return Ok(result);
        }
    }
}
