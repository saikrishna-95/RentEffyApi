using Dapper;
using Microsoft.AspNetCore.Connections;
using Renteffy.Domain.DTOs.Owner;
using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Domain.DTOs.Owner.Response;
using Renteffy.Domain.DTOs.User.Response;
using Renteffy.Domain.DTOs.UserTrans.Response;
using Renteffy.Domain.Entities.Registration;
using Renteffy.Domain.Services.PersistanceInterfaces.Owner;
using Renteffy.Shared.Database.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Renteffy.Persistence.Implementation.Owner
{
    public class GetPostsByOwnerPersistance : IGetPostsByOwnerPersistance
    {
        private readonly IDbConnectionFactory _dbFactory;
        public GetPostsByOwnerPersistance(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async Task<List<PublicPostResponseDto>> GetPostsByOwnerIdAsync(int ownerId)
        {
            using var con = _dbFactory.CreateConnection();

            using var multi = await con.QueryMultipleAsync(
                "sp_GetPostsByOwnerId",
                new { OwnerId = ownerId },
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

        public async Task<EditOwnerPostResponseDto> GetPostForEditAsync(int postId)
        {
            using var connection = _dbFactory.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                                    "sp_GetPostForEdit",
                                    new { PostId = postId },
                                    commandType: CommandType.StoredProcedure);

            var post = await multi.ReadFirstOrDefaultAsync<EditOwnerPostResponseDto>();
            post.Media = (await multi.ReadAsync<PostMediaDto>()).ToList();
            post.RoomPricing = (await multi.ReadAsync<RoomPricingDto>()).ToList();
            post.Amenities = (await multi.ReadAsync<AmenitiesDto>()).ToList();
            post.StayingPeriods = (await multi.ReadAsync<StayingPeriodsPostDto>()).ToList();
            post.FoodPosts = (await multi.ReadAsync<FoodPostDto>()).ToList();

            return post;
        }
    }
}
