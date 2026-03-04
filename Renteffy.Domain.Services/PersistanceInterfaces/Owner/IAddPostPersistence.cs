using Renteffy.Domain.DTOs.Owner.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.PersistanceInterfaces.Owner
{
    public interface IAddPostPersistence
    {
        Task<int> AddPostAsync(AddPostRequestDto request);
        Task SavePostMediaAsync(List<PostMediaDto> media);
    }
}
