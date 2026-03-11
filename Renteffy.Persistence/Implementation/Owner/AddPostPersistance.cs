using Dapper;
using Microsoft.AspNetCore.Http;
using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Domain.Services.PersistanceInterfaces.Owner;
using Renteffy.Shared.Database.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Renteffy.Persistence.Implementation.Owner
{
    public class AddPostPersistance : IAddPostPersistence
    {
        private readonly IDbConnectionFactory _dbFactory;

        public AddPostPersistance(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async Task<int> AddPostAsync(AddPostRequestDto request)
        {
            using var con = _dbFactory.CreateConnection();

            // 🔹 Build TVP DataTable
            var pricingTable = new DataTable();
            pricingTable.Columns.Add("FloorId", typeof(int));
            pricingTable.Columns.Add("RoomId", typeof(int));
            pricingTable.Columns.Add("BedTypeId", typeof(int));
            pricingTable.Columns.Add("Price", typeof(decimal));
            pricingTable.Columns.Add("IsAvailable", typeof(bool));

            foreach (var rp in request.RoomPricing)
            {
                pricingTable.Rows.Add(
                    rp.FloorId,
                    rp.RoomId,
                    rp.BedTypeId,
                    rp.Price,
                    rp.IsAvailable
                );
            }

            var amenitiesTable = new DataTable();
            amenitiesTable.Columns.Add("AmenityId", typeof(int));

            foreach (var rp in request.Amenities)
            {
                amenitiesTable.Rows.Add(
                    rp.AmenityId
                );
            }

            var stayingPeriodTable = new DataTable();
            stayingPeriodTable.Columns.Add("StngPrdId", typeof(int));

            foreach (var rp in request.StayingPeriods)
            {
                stayingPeriodTable.Rows.Add(
                    rp.StngPrdId
                );
            }

            var foodPostTable = new DataTable();
            foodPostTable.Columns.Add("FoodId", typeof(int));

            foreach (var rp in request.FoodPosts)
            {
                foodPostTable.Rows.Add(
                    rp.FoodId
                );
            }

            var parameters = new DynamicParameters();

            parameters.Add("@OwnerId", request.UserId);
            parameters.Add("@CategoryId", request.CategoryId);
            parameters.Add("@PgTypeId", request.PgTypeId);  
            parameters.Add("@PgName", request.PgName);
            parameters.Add("@ApartmentName", request.ApartmentName);
            parameters.Add("@Address", request.Address);
            parameters.Add("@Pincode", request.Pincode);
            parameters.Add("@Mobile", request.Mobile);
            parameters.Add("@Latitude", request.Latitude);
            parameters.Add("@Longitude", request.Longitude);
            parameters.Add("@TotalFloors", request.TotalFloors);
            parameters.Add("@TotalRooms", request.TotalRooms);
            parameters.Add("@Status", 1);
            // 🔑 TVP parameter
            parameters.Add("@RoomPricing", pricingTable.AsTableValuedParameter("RoomPricingList"));
            parameters.Add("@Amenities", amenitiesTable.AsTableValuedParameter("AmenityList"));
            parameters.Add("@StayingPeriods", stayingPeriodTable.AsTableValuedParameter("StayPeriodList"));
            parameters.Add("@FoodPosts", foodPostTable.AsTableValuedParameter("FoodPostList"));

            var postId = await con.QuerySingleAsync<int>("sp_AddPostWithRoomPricing", parameters, commandType: CommandType.StoredProcedure);

            return postId;
        }

        public async Task SavePostMediaAsync(List<PostMediaDto> media)
        {
            using var con = _dbFactory.CreateConnection();

            await con.ExecuteAsync(@"
                INSERT INTO T_PostMedia_TR
                (PostId, MediaType, FileName, FilePath, ContentType)
                VALUES
                (@PostId, @MediaType, @FileName, @FilePath, @ContentType)",
                media
            );
        }

    }
}
