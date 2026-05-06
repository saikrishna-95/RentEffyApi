using Microsoft.AspNetCore.Http;
using Renteffy.Domain.DTOs.Owner.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Interfaces.Owner
{
    public interface IAddPostApplication
    {
        Task<int> AddPostAsync(AddPostRequestDto request, List<IFormFile> files);
        Task<int> UpdatePostAsync(UpdatePostRequestDTO request, List<(int mediaId, IFormFile file)> replaceMedia, List<IFormFile> newFiles);
        Task<bool> DeletePostAsync(int postId, int userId);
        Task<bool> UpdatePostStatusAsync(int postId, int userId, int status);
    }
}
