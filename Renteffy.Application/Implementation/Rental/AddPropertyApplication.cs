using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Renteffy.Application.Interfaces.Rental;
using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs;
using Renteffy.Domain.DTOs.UserTrans.Request;
using Renteffy.Domain.Services.Interfaces.Owner;
using Renteffy.Domain.Services.Interfaces.Rental;
using Renteffy.Domain.Services.PersistanceInterfaces.Owner;
using Renteffy.Domain.Services.PersistanceInterfaces.Rental;
using Renteffy.Shared.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Implementation.Rental
{
    public class AddPropertyApplication:IAddPropertyApplication
    {
        private readonly IAddPropertyDomain  _domn;
        private readonly IAddPostPropertyPersistence _repo;
        private readonly IConfiguration _config;
        private readonly FileStorageOptions _fileOptions;
        private readonly IWebHostEnvironment _environment;
        private readonly Cloudinary _cloudinary;
        public AddPropertyApplication(IAddPostPropertyPersistence repo, IAddPropertyDomain domn, IConfiguration config, IOptions<FileStorageOptions> options,
        IWebHostEnvironment environment, Cloudinary cloudinary)
        {
            _repo = repo;
            _domn = domn;
            _config = config;
            _fileOptions = options.Value;
            _environment = environment;
            _cloudinary = cloudinary;
        }
        public async Task<int> AddProperty(AddPropertyRequestDto request, List<IFormFile> files, List<PropertyMediaDto> mediaMeta)
        {
            var propertypostid = await _domn.AddProperty(request);
            if (propertypostid > 0)
            {
                if (files != null && files.Count > 0)
                {
                    var media = await SaveFilesAsync(request.UserId, propertypostid, files, mediaMeta);

                    await _repo.SavePropertyMediaAsync(media);
                }
            }
            return propertypostid;
        }

        private async Task<List<PropertyPostMediaDto>> SaveFilesAsync(int userid, int propertypostid, List<IFormFile> files, List<PropertyMediaDto> mediaMeta)
        {
            var mediaList = new List<PropertyPostMediaDto>();

            foreach (var file in files)
            {
                var meta = mediaMeta.FirstOrDefault(x => x.FileName == file.FileName);

                if (meta == null)
                    continue;

                await using var stream = file.OpenReadStream();

                RawUploadResult uploadResult;

                if (file.ContentType.StartsWith("video"))
                {
                    var uploadParams = new VideoUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = $"propertiesmedia/{propertypostid}"
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
                else
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = $"propertiesmedia/{propertypostid}"
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }

                mediaList.Add(new PropertyPostMediaDto
                {
                    PropertyPostID = propertypostid,
                    PropertyMediaTypeID = meta.PropertyMediaTypeID,
                    MediaType = file.ContentType.StartsWith("video") ? "Video" : "Image",
                    FileName = uploadResult.PublicId,
                    FilePath = uploadResult.SecureUrl.ToString(),
                    ContentType = file.ContentType,
                    CreatedBy = userid
                });
            }

            return mediaList;
        }

        public async Task<int> UpdatePropertyAsync(UpdatePropertyRequestDto request, List<(int mediaId, IFormFile file)> replaceMedia, List<IFormFile> newFiles)
        {
            return await _domn.UpdatePropertyAsync(request, replaceMedia, newFiles);
        }

        public async Task<bool> UpdatePropertyStatusAsync(int propertyId, int userId, int status)
        {
            return await _domn.UpdatePropertyStatusAsync(propertyId, userId, status);
        }

        public async Task<bool> DeletePropertyAsync(int propertyId, int userId)
        {
            return await _domn.DeletePropertyAsync(propertyId, userId);
        }
    }
}
