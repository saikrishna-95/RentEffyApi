using Microsoft.Extensions.Configuration;
using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Domain.DTOs.User.Response;
using Renteffy.Domain.DTOs.UserTrans.Response;
using Renteffy.Domain.Services.Interfaces.User;
using Renteffy.Domain.Services.PersistanceInterfaces.Owner;
using Renteffy.Domain.Services.PersistanceInterfaces.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Renteffy.Domain.Services.Implementation.User
{
    public class GetOwnerPostsDomain:IGetOwnerPostsDomain
    {
        private readonly IGetOwnerPostsPersistence _readRepo;
        private readonly IConfiguration _config;

        public GetOwnerPostsDomain(IGetOwnerPostsPersistence readRepo, IConfiguration config)
        {
            _readRepo = readRepo;
            _config = config;
        }
        public async Task<List<PublicPostResponseDto>> GetPublicPostsAsync()
            => await _readRepo.GetPublicPostsAsync();

        public async Task<List<ProductsResponseDTO>> GetProductsAsync(int postid)
            => await _readRepo.GetProductsAsync(postid);

        public async Task<int> LikePostAsync(int postId, int userId)
        {
            return await _readRepo.LikePostAsync(postId, userId);
        }

        public async Task<LikesCountResponseDTO> GetLikesCountAsync(int postId, int userId)
        {
            return await _readRepo.GetLikesCountAsync(postId, userId);
        }

        public async Task<int> FavoritePostAsync(int postId, int userId)
        {
            return await _readRepo.FavoritePostAsync(postId, userId);
        }

        public async Task<FavoritesCountResponseDTO> GetFavoritesCountAsync(int postId, int userId)
        {
            return await _readRepo.GetFavoritesCountAsync(postId, userId);
        }

        public async Task<int> AddCommentAsync(int postId, int userId, string comment)
        {
           return await _readRepo.AddCommentAsync(postId, userId, comment);
        }

        public async Task<IEnumerable<dynamic>> GetCommentsAsync(int postId)
        {
            return await _readRepo.GetCommentsAsync(postId);
        }

        public async Task<int> SendRequestAsync(int postId, int userId, string message)
        {
            return await _readRepo.SendRequestAsync(postId, userId, message);
        }

        public async Task<IEnumerable<dynamic>> GetOwnerRequestsAsync(int ownerId)
        {
            return await _readRepo.GetOwnerRequestsAsync(ownerId);
        }

    }
}
