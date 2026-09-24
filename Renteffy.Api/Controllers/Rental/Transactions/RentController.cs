using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renteffy.Application.Implementation.Rental;
using Renteffy.Application.Interfaces.Owner;
using Renteffy.Application.Interfaces.Rental;
using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs;
using System.Text.Json;

namespace Renteffy.Api.Controllers.Rental.Transactions
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly IAddPropertyApplication _readApp;
        private readonly IGetAllPropertiesApplication _getAllPropertiesApplication;
        public RentController(IAddPropertyApplication readApp, IGetAllPropertiesApplication getAllPropertiesApplication)
        {
            _readApp = readApp;
            _getAllPropertiesApplication = getAllPropertiesApplication;
        }

        [AllowAnonymous]
        [HttpGet("GetAllProperties")]
        public async Task<IActionResult> GetAllProperties()
        {
            var properties = await _getAllPropertiesApplication.GetPublicPostPropertiesAsync();

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            foreach (var property in properties)
            {
                if (property.Media != null)
                {
                    foreach (var media in property.Media)
                    {
                        media.FileUrl = $"{media.FilePath}";
                    }
                }
            }

            return Ok(properties);
        }

        [AllowAnonymous]
        [HttpPost("AddProperty")]
        public async Task<IActionResult> AddProperty([FromForm] string data, [FromForm] List<IFormFile> files)
        {
            try
            {
                var request = JsonSerializer.Deserialize<AddPropertyRequestDto>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                //var mediaMeta = JsonSerializer.Deserialize<List<MediaMetaDto>>(mediaMetaData,new JsonSerializerOptions { PropertyNameCaseInsensitive = true,AllowTrailingCommas=true });

                if (request == null)
                    return BadRequest(new
                    {
                        success = false,
                        message = "Invalid property data"
                    });

                // Validate Media Metadata
                if (request.Media == null ||
                    request.Media.Count == 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Media metadata required"
                    });
                }

                if (files == null || files.Count == 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Files required"
                    });
                }

                // Files count must match metadata count
                if (files.Count != request.Media.Count)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Files count and media metadata count mismatch"
                    });
                }
                var PropertyPostID = await _readApp.AddProperty(request, files, request.Media);

                return Ok(new
                {
                    Success = PropertyPostID <= 0 ? false : true,
                    PropertyPostID = PropertyPostID
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // This will show the real error
            }
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpPost("UpdateProperty")]
        public async Task<IActionResult> UpdatePropertyAsync([FromForm] string data,[FromForm] List<IFormFile> newFiles,[FromForm] List<int> replaceMediaIds,[FromForm] List<IFormFile> replaceMediaFiles)
        {
            try
            {
                var request = JsonSerializer.Deserialize<UpdatePropertyRequestDto>(data,new JsonSerializerOptions{PropertyNameCaseInsensitive = true});

                if (request == null)
                    return BadRequest("Invalid property data");

                newFiles ??= new List<IFormFile>();
                replaceMediaIds ??= new List<int>();
                replaceMediaFiles ??= new List<IFormFile>();

                if (replaceMediaIds.Count != replaceMediaFiles.Count)
                {
                    return BadRequest("Replace media IDs and files count must be the same.");
                }

                var replaceMedia = replaceMediaIds.Zip(replaceMediaFiles,(id, file) => (mediaId: id, file)).ToList();
                var propertyId = await _readApp.UpdatePropertyAsync(request,replaceMedia,newFiles);

                return Ok(new
                {
                    Success = propertyId > 0,
                    PropertyId = propertyId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpPost("UpdatePropertyStatus")]
        public async Task<IActionResult> UpdatePropertyStatus(int propertyId,int userId,int status)
        {
            var result = await _readApp.UpdatePropertyStatusAsync(propertyId,userId,status);

            if (!result)
                return BadRequest("Failed");

            return Ok(new
            {
                message = "Property status updated successfully",
                Success = true,
                Result = 1
            });
        }

        [AllowAnonymous]
        [HttpGet("GetPropertyById")]
        public async Task<IActionResult> GetPropertyByIdAsync(int propertyid)
        {
            var properties = await _getAllPropertiesApplication.GetPropertyByIdAsync(propertyid);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            foreach (var property in properties)
            {
                foreach (var media in property.Media)
                {
                    // media.FileUrl = $"{baseUrl}{media.FilePath}";
                    media.FileUrl = $"{media.FilePath}";
                }
            }

            return Ok(properties);
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpPost("GetPropertiesByOwnerId")]
        public async Task<IActionResult> GetPropertiesByOwnerId(int ownerId)
        {
            if (ownerId <= 0)
                return BadRequest("Invalid owner data");

            var properties = await _getAllPropertiesApplication.GetPropertiesByOwnerIdAsync(ownerId);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            foreach (var property in properties)
            {
                foreach (var media in property.Media)
                {
                    //media.FileUrl = $"{baseUrl}{media.FilePath}";
                    media.FileUrl = $"{media.FilePath}";
                }
            }

            return Ok(properties);
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpPost("GetPropertyForEdit")]
        public async Task<IActionResult> GetPropertyForEdit(int propertyId)
        {
            if (propertyId <= 0)
                return BadRequest("Invalid data enter.");

            var property = await _getAllPropertiesApplication.GetPropertyForEditAsync(propertyId);

            if (property == null)
                return NotFound();

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            if (property.Media != null)
            {
                foreach (var media in property.Media)
                {
                    //media.FileUrl = $"{baseUrl}/{media.FileUrl}";
                    media.FileUrl = $"{media.FilePath}";
                }
            }

            return Ok(property);
        }

        //[Authorize]
        [AllowAnonymous]
        [HttpPost("DeletePropertyById")]
        public async Task<IActionResult> DeleteProperty(int propertyId, int userId)
        {
            var result = await _readApp.DeletePropertyAsync(propertyId, userId);

            if (!result)
                return BadRequest(new
                {
                    Success = false,
                    message = "Delete failed",
                    Result = 0
                });

            return Ok(new
            {
                Success = true,
                message = "Property deleted successfully",
                Result = 1
            });
        }
    }
}
