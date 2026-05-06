using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Renteffy.Application.Interfaces.Owner;
using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Domain.Services.Interfaces.Owner;
using Renteffy.Domain.Services.PersistanceInterfaces.Owner;
using Renteffy.Shared.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Implementation.Owner
{
    public class AddPostApplication:IAddPostApplication
    {
        private readonly IAddPostDomain _domn;
        private readonly IAddPostPersistence _repo;
        private readonly IConfiguration _config;
        private readonly FileStorageOptions _fileOptions;
        private readonly IWebHostEnvironment _environment;
        private readonly Cloudinary _cloudinary;

        public AddPostApplication(IAddPostPersistence repo,IAddPostDomain domn, IConfiguration config, IOptions<FileStorageOptions> options,
        IWebHostEnvironment environment, Cloudinary cloudinary)
        {
            _repo = repo;
            _domn = domn;
            _config = config;
            _fileOptions = options.Value;
            _environment = environment;
            _cloudinary = cloudinary;
        }
        public async Task<int> AddPostAsync(AddPostRequestDto request, List<IFormFile> files)
        {
            var postId = await _domn.AddPostAsync(request);
            if (postId > 0)
            {
                if (files != null && files.Count > 0)
                {
                    var media = await SaveFilesAsync(postId, files);

                    await _repo.SavePostMediaAsync(media);
                }
            }
            return postId;
        }

        private async Task<List<PostMediaDto>> SaveFilesAsync(int postId, List<IFormFile> files)
        {
            var mediaList = new List<PostMediaDto>();

            foreach (var file in files)
            {
                await using var stream = file.OpenReadStream();

                RawUploadResult uploadResult;

                if (file.ContentType.StartsWith("video"))
                {
                    var uploadParams = new VideoUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = $"posts/{postId}"
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
                else
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = $"posts/{postId}"
                    };

                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }

                mediaList.Add(new PostMediaDto
                {
                    PostId = postId,
                    MediaType = file.ContentType.StartsWith("video") ? "Video" : "Image",
                    FileName = uploadResult.PublicId,
                    FilePath = uploadResult.SecureUrl.ToString(),
                    ContentType = file.ContentType
                });
            }

            return mediaList;
        }

        public async Task<int> UpdatePostAsync(UpdatePostRequestDTO request, List<(int mediaId, IFormFile file)> replaceMedia, List<IFormFile> newFiles)
            => await _domn.UpdatePostAsync(request, replaceMedia, newFiles);

        public async Task<bool> DeletePostAsync(int postId, int userId)
            => await _domn.DeletePostAsync(postId, userId);

        public async Task<bool> UpdatePostStatusAsync(int postId, int userId, int status)
            => await _domn.UpdatePostStatusAsync(postId, userId, status);
    }
}
