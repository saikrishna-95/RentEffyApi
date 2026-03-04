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
    }
}
