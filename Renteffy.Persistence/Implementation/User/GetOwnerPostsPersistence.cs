using Dapper;
using Renteffy.Domain.DTOs.Owner;
using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Domain.DTOs.User.Response;
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

    }
}
