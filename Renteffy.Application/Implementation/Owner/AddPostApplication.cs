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

        public AddPostApplication(IAddPostPersistence repo,IAddPostDomain domn, IConfiguration config, IOptions<FileStorageOptions> options,
        IWebHostEnvironment environment)
        {
            _repo = repo;
            _domn = domn;
            _config = config;
            _fileOptions = options.Value;
            _environment = environment;
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
        private async Task<List<PostMediaDto>> SaveFilesAsync(int postId,List<IFormFile> files)
        {
            //var rootPath = Path.Combine(
            //    _environment.ContentRootPath,
            //    _fileOptions.FolderName
            //);

            string rootPath;

            if (_environment.IsDevelopment())
            {
                rootPath = Path.Combine("C:\\", _fileOptions.FolderName);
            }
            else
            {
                rootPath = Path.Combine(Directory.GetParent(_environment.ContentRootPath)!.FullName,_fileOptions.FolderName);
            }

            var uploadRoot = Path.Combine(rootPath,"posts",postId.ToString());

            if (!Directory.Exists(uploadRoot))
                Directory.CreateDirectory(uploadRoot);

            var mediaList = new List<PostMediaDto>();

            foreach (var file in files)
            {
                var ext = Path.GetExtension(file.FileName);
                var fileName = $"{Guid.NewGuid()}{ext}";
                var fullPath = Path.Combine(uploadRoot, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);

                mediaList.Add(new PostMediaDto
                {
                    PostId = postId,
                    MediaType = file.ContentType.StartsWith("video") ? "Video" : "Image",
                    FileName = fileName,
                    FilePath = $"{_fileOptions.PublicBaseUrl}/posts/{postId}/{fileName}",
                    ContentType = file.ContentType
                });
            }

            return mediaList;
        }
    }
}
