using Renteffy.Domain.DTOs.User.Response;
using Renteffy.Domain.DTOs.UserTrans.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Application.Interfaces.User
{
    public interface IGetOwnerPostsApplication
    {
        Task<List<PublicPostResponseDto>> GetPublicPostsAsync();
        Task<List<ProductsResponseDTO>> GetProductsAsync(int postid);
        Task<int> LikePostAsync(int postId, int userId);
        Task<int> FavoritePostAsync(int postId, int userId);
        Task<int> AddCommentAsync(int postId, int userId, string comment);
        Task<IEnumerable<dynamic>> GetCommentsAsync(int postId);
        Task<int> SendRequestAsync(int postId, int userId, string message);
        Task<IEnumerable<dynamic>> GetOwnerRequestsAsync(int ownerId);
        Task<LikesCountResponseDTO> GetLikesCountAsync(int postId, int userId);
        Task<FavoritesCountResponseDTO> GetFavoritesCountAsync(int postId, int userId);
    }
}
