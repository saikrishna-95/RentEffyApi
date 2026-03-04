using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public OwnerTransactionsController(IAddPostApplication readApp) => _readApp = readApp;

        [AllowAnonymous]
        [HttpPost("AddPost")]
        public async Task<IActionResult> AddPostAsync([FromForm] AddPostRequestDto request, [FromForm] List<IFormFile> files)
        {
            //var request = JsonSerializer.Deserialize<AddPostRequestDto>(postData,
            //            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (request == null)
                return BadRequest("Invalid post data");

            var postId = await _readApp.AddPostAsync(request,files);

            return Ok(new
            {
                Success = postId <= 0 ? false : true,
                PostId = postId
            });
        }
    }
}
