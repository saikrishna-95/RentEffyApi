using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Renteffy.Application.Interfaces.Owner;
using Renteffy.Application.Interfaces.PasswordRestChange;
using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Shared.Security;
using System.Text.Json;

namespace Renteffy.Api.Controllers.Owner
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerTransactionsController : ControllerBase
    {
        private readonly IAddPostApplication _readApp;
        private readonly IGetPostsByOwnerApplication _getPostsByOwnerApplication;
        public OwnerTransactionsController(IAddPostApplication readApp, IGetPostsByOwnerApplication getPostsByOwnerApplication)
        {
            _readApp = readApp;
            _getPostsByOwnerApplication = getPostsByOwnerApplication;
        }

        [AllowAnonymous]
        [HttpPost("AddPost")]
        public async Task<IActionResult> AddPostAsync([FromForm] string data, [FromForm] List<IFormFile> files)
        {
            try
            {
                var request = JsonSerializer.Deserialize<AddPostRequestDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (request == null)
                    return BadRequest("Invalid post data");

                var postId = await _readApp.AddPostAsync(request, files);

                return Ok(new
                {
                    Success = postId <= 0 ? false : true,
                    PostId = postId
                });
                // your existing code
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // This will show the real error
            }
        }

        [Authorize]
        //[AllowAnonymous]
        [HttpPost("GetPostsByOwnerId")]
        public async Task<IActionResult> GetPostsByOwnerId(int ownerId)
        {
            if (ownerId==null)
                return BadRequest("Invalid post data");

            var posts = await _getPostsByOwnerApplication.GetPostsByOwnerIdAsync(ownerId);
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

        [Authorize]
        //[AllowAnonymous]
        [HttpPost("GetPostForEdit")]
        public async Task<IActionResult> GetPostForEdit(int postId)
        {
            if (postId == null)
                return BadRequest("Invalid data enter.");

            var posts = await _getPostsByOwnerApplication.GetPostForEditAsync(postId);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            foreach (var media in posts.Media)
            {
                media.FileUrl = $"{baseUrl}/{media.FileUrl}";
            }
            if (posts == null)
                return NotFound();

            return Ok(posts);
        }
    }
}
