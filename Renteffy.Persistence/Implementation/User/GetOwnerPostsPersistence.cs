using Dapper;
using Renteffy.Domain.DTOs.Owner;
using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Domain.DTOs.Owner.Response;
using Renteffy.Domain.DTOs.User.Response;
using Renteffy.Domain.DTOs.UserTrans.Response;
using Renteffy.Domain.Services.PersistanceInterfaces.User;
using Renteffy.Shared.Database.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Renteffy.Persistence.Implementation.User
{
    public class GetOwnerPostsPersistence:IGetOwnerPostsPersistence
    {
        private readonly IDbConnectionFactory _dbFactory;

        public GetOwnerPostsPersistence(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async Task<List<PublicPostResponseDto>> GetPublicPostsAsync()
        {
            using var con = _dbFactory.CreateConnection();

            using var multi = await con.QueryMultipleAsync(
                "sp_GetPublicPosts",
                commandType: CommandType.StoredProcedure
            );

            var posts = (await multi.ReadAsync<PublicPostResponseDto>()).ToList();
            var media = (await multi.ReadAsync<PostMediaDto>()).ToList();
            var pricing = (await multi.ReadAsync<RoomPricingDto>()).ToList();
            var amenities = (await multi.ReadAsync<AmenitiesDto>()).ToList();
            var stayingperiods = (await multi.ReadAsync<StayingPeriodsPostDto>()).ToList();
            var food = (await multi.ReadAsync<FoodPostDto>()).ToList();

            foreach (var post in posts)
            {
                post.Media = media.Where(m => m.PostId == post.PostId).ToList();
                post.Pricing = pricing.Where(p => p.PostId == post.PostId).ToList();
                post.Amenities = amenities.Where(p => p.PostId == post.PostId).ToList();
                post.stayingPeriods = stayingperiods.Where(p => p.PostId == post.PostId).ToList();
                post.foodPosts = food.Where(p => p.PostId == post.PostId).ToList();
            }

            return posts;
        }

        public async Task<List<ProductsResponseDTO>> GetProductsAsync(int postid)
        {
            using var con = _dbFactory.CreateConnection();

            using var multi = await con.QueryMultipleAsync(
                "sp_GetProducts",
                new { PostId = postid },
                commandType: CommandType.StoredProcedure
            );

            var posts = (await multi.ReadAsync<ProductsResponseDTO>()).ToList();
            var media = (await multi.ReadAsync<PostMediaDto>()).ToList();
            var pricing = (await multi.ReadAsync<ProcuctRoomPriceingReponseDTO>()).ToList();
            var amenities = (await multi.ReadAsync<AmenitiesDto>()).ToList();
            var stayingperiods = (await multi.ReadAsync<StayingPeriodsPostDto>()).ToList();
            var food = (await multi.ReadAsync<FoodPostDto>()).ToList();

            foreach (var post in posts)
            {
                post.Media = media.Where(m => m.PostId == post.PostId).ToList();
                post.Pricing = pricing.Where(p => p.PostId == post.PostId).ToList();
                post.Amenities = amenities.Where(p => p.PostId == post.PostId).ToList();
                post.stayingPeriods = stayingperiods.Where(p => p.PostId == post.PostId).ToList();
                post.foodPosts = food.Where(p => p.PostId == post.PostId).ToList();
            }

            return posts;
        }

        public async Task<int> LikePostAsync(int postId, int userId)
        {
            using var con = _dbFactory.CreateConnection();

            var result = await con.QueryFirstAsync<int>(
                "sp_LikePost",
                new { PostId = postId, UserId = userId },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<LikesCountResponseDTO> GetLikesCountAsync(int postId, int userId)
        {
            using var con = _dbFactory.CreateConnection();

            var result = await con.QueryFirstAsync<LikesCountResponseDTO>(
                "sp_GetLikesCount",
                new { PostId = postId, UserId = userId },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<int> FavoritePostAsync(int postId, int userId)
        {
            using var con = _dbFactory.CreateConnection();

            var result = await con.QueryFirstAsync<int>(
                "sp_FavoritePost",
                new { PostId = postId, UserId = userId },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<FavoritesCountResponseDTO> GetFavoritesCountAsync(int postId, int userId)
        {
            using var con = _dbFactory.CreateConnection();

            var result = await con.QueryFirstAsync<FavoritesCountResponseDTO>(
                "sp_GetFavoritesCount",
                new { PostId = postId, UserId = userId },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<int> AddCommentAsync(int postId, int userId, string comment)
        {
            using var con = _dbFactory.CreateConnection();

            var id = await con.QueryFirstAsync<int>(
                "sp_AddComment",
                new { PostId = postId, UserId = userId, Comment = comment },
                commandType: CommandType.StoredProcedure
            );

            return id;
        }

        public async Task<IEnumerable<dynamic>> GetCommentsAsync(int postId)
        {
            using var con = _dbFactory.CreateConnection();

            var list = await con.QueryAsync(
                "sp_GetComments",
                new { PostId = postId },
                commandType: CommandType.StoredProcedure
            );

            return list;
        }

        public async Task<int> SendRequestAsync(int postId, int userId, string message)
        {
            using var con = _dbFactory.CreateConnection();

            var result = await con.QueryFirstAsync<int>(
                "sp_SendRequest",
                new { PostId = postId, UserId = userId, Message = message },
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<IEnumerable<dynamic>> GetOwnerRequestsAsync(int ownerId)
        {
            using var con = _dbFactory.CreateConnection();

            var list = await con.QueryAsync(
                "sp_GetOwnerRequests",
                new { OwnerId = ownerId },
                commandType: CommandType.StoredProcedure
            );

            return list;
        }

    }
}
