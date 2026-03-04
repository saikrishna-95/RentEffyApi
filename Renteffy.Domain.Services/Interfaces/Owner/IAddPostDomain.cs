using Renteffy.Domain.DTOs.Owner.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Interfaces.Owner
{
    public interface IAddPostDomain
    {
        Task<int> AddPostAsync(AddPostRequestDto request);
    }
}
