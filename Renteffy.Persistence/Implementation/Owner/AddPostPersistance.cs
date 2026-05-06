using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
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
        private readonly Cloudinary _cloudinary;

        public AddPostPersistance(IDbConnectionFactory dbFactory, Cloudinary cloudinary)
        {
            _dbFactory = dbFactory;
            _cloudinary = cloudinary;
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
            parameters.Add("@HouseNo", request.HouseNo);
            parameters.Add("@Street", request.Street);
            parameters.Add("@AreaName", request.AreaName);
            parameters.Add("@City", request.City);
            parameters.Add("@State", request.State);
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

        private async Task<List<UpdatePostMediaDto>> GetMediaByIds(List<int> mediaIds)
        {
            using var con = _dbFactory.CreateConnection();

            return (await con.QueryAsync<UpdatePostMediaDto>(
                "SELECT * FROM T_PostMedia_TR WHERE mediaId IN @MediaIds",
                new { MediaIds = mediaIds }
            )).ToList();
        }

        private async Task DeleteFromCloudinary(UpdatePostMediaDto media)
        {
            if (media.MediaType == "Image")
            {
                await _cloudinary.DestroyAsync(new DeletionParams(media.FileName));
            }
            else
            {
                await _cloudinary.DestroyAsync(new DeletionParams(media.FileName)
                {
                    ResourceType = ResourceType.Video
                });
            }
        }

        private async Task DeleteMediaAsync(List<int> mediaIds)
        {
            if (mediaIds == null || !mediaIds.Any()) return;

            using var con = _dbFactory.CreateConnection();

            var mediaList = await GetMediaByIds(mediaIds);

            foreach (var media in mediaList)
            {
                await DeleteFromCloudinary(media);
            }

            await con.ExecuteAsync(
                "DELETE FROM T_PostMedia_TR WHERE MediaId IN @MediaIds",
                new { MediaIds = mediaIds }
            );
        }

        private async Task<List<UpdatePostMediaDto>> ReplaceMediaAsync(int postId,List<(int mediaId, IFormFile file)> replaceList)
        {
            var updatedList = new List<UpdatePostMediaDto>();

            if (replaceList == null || !replaceList.Any())
                return updatedList;

            var ids = replaceList.Select(x => x.mediaId).ToList();
            var existingMedia = await GetMediaByIds(ids);

            foreach (var item in replaceList)
            {
                var oldMedia = existingMedia.FirstOrDefault(x => x.MediaId == item.mediaId);
                if (oldMedia == null) continue;

                // delete old
                await DeleteFromCloudinary(oldMedia);

                // upload new
                await using var stream = item.file.OpenReadStream();

                RawUploadResult uploadResult;

                if (item.file.ContentType.StartsWith("video"))
                {
                    uploadResult = await _cloudinary.UploadAsync(new VideoUploadParams
                    {
                        File = new FileDescription(item.file.FileName, stream),
                        Folder = $"posts/{postId}"
                    });
                }
                else
                {
                    uploadResult = await _cloudinary.UploadAsync(new ImageUploadParams
                    {
                        File = new FileDescription(item.file.FileName, stream),
                        Folder = $"posts/{postId}"
                    });
                }

                updatedList.Add(new UpdatePostMediaDto
                {
                    MediaId = oldMedia.MediaId,
                    PostId = postId,
                    MediaType = item.file.ContentType.StartsWith("video") ? "Video" : "Image",
                    FileName = uploadResult.PublicId,
                    FilePath = uploadResult.SecureUrl.ToString(),
                    ContentType = item.file.ContentType
                });
            }

            return updatedList;
        }

        private async Task<List<PostMediaDto>> AddNewMediaAsync(int postId, List<IFormFile> files)
        {
            var mediaList = new List<PostMediaDto>();

            foreach (var file in files)
            {
                await using var stream = file.OpenReadStream();

                RawUploadResult uploadResult;

                if (file.ContentType.StartsWith("video"))
                {
                    uploadResult = await _cloudinary.UploadAsync(new VideoUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = $"posts/{postId}"
                    });
                }
                else
                {
                    uploadResult = await _cloudinary.UploadAsync(new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = $"posts/{postId}"
                    });
                }

                mediaList.Add(new PostMediaDto
                {
                    PostId = postId,
                    MediaType = file.ContentType.StartsWith("video") ? "Video" : "Image",
                    FileName = uploadResult.PublicId,
                    FilePath = uploadResult.SecureUrl.ToString(),
                    ContentType = file.ContentType
                });
            }

            return mediaList;
        }

        public async Task UpdatePostMediaAsync(List<UpdatePostMediaDto> mediaList)
        {
            using var con = _dbFactory.CreateConnection();

            foreach (var media in mediaList)
            {
                await con.ExecuteAsync(@"
                UPDATE T_PostMedia_TR
                SET
                    MediaType = @MediaType,
                    FileName = @FileName,
                    FilePath = @FilePath,
                    ContentType = @ContentType
                WHERE Id = @Id",
                        media
                    );
            }
        }

        public async Task<int> UpdatePostAsync(UpdatePostRequestDTO request,List<(int mediaId, IFormFile file)> replaceMedia,List<IFormFile> newFiles)
        {
            using var con = _dbFactory.CreateConnection();

            // Build TVP DataTable
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

            // 🔹 1. Update Post (SP call)
            var parameters = new DynamicParameters();
            parameters.Add("@PostId", request.PostId);
            parameters.Add("@OwnerId", request.UserId);
            parameters.Add("@CategoryId", request.CategoryId);
            parameters.Add("@PgTypeId", request.PgTypeId);
            parameters.Add("@PgName", request.PgName);
            parameters.Add("@ApartmentName", request.ApartmentName);

            parameters.Add("@HouseNo", request.HouseNo);
            parameters.Add("@Street", request.Street);
            parameters.Add("@AreaName", request.AreaName);
            parameters.Add("@City", request.City);
            parameters.Add("@State", request.State);
            parameters.Add("@Pincode", request.Pincode);

            parameters.Add("@Mobile", request.Mobile);
            parameters.Add("@Latitude", request.Latitude);
            parameters.Add("@Longitude", request.Longitude);
            parameters.Add("@TotalFloors", request.TotalFloors);
            parameters.Add("@TotalRooms", request.TotalRooms);

            parameters.Add("@RoomPricing", pricingTable.AsTableValuedParameter("RoomPricingList"));
            parameters.Add("@Amenities", amenitiesTable.AsTableValuedParameter("AmenityList"));
            parameters.Add("@StayingPeriods", stayingPeriodTable.AsTableValuedParameter("StayPeriodList"));
            parameters.Add("@FoodPosts", foodPostTable.AsTableValuedParameter("FoodPostList"));

            var postId = await con.QuerySingleAsync<int>(
                "sp_UpdatePostWithRoomPricing",
                parameters,
                commandType: CommandType.StoredProcedure);

            // DELETE selected media
            await DeleteMediaAsync(request.DeleteMediaIds);

            // REPLACE media
            var replaced = await ReplaceMediaAsync(postId, replaceMedia);
            await UpdatePostMediaAsync(replaced);

            // ADD new media
            var added = await AddNewMediaAsync(postId, newFiles);
            await SavePostMediaAsync(added);

            return postId;
        }

        private async Task<List<PostMediaDto>> GetPostMediaAsync(int postId)
        {
            using var con = _dbFactory.CreateConnection();

            var media = await con.QueryAsync<PostMediaDto>(
                @"SELECT PostId, MediaType, FileName, FilePath, ContentType 
                    FROM T_PostMedia_TR 
                   WHERE PostId = @PostId",
                new { PostId = postId });

            return media.ToList();
        }

        private async Task DeleteMediaFromCloudinaryAsync(List<PostMediaDto> mediaList)
        {
            if (mediaList == null || !mediaList.Any()) return;

            foreach (var media in mediaList)
            {
                try
                {
                    if (media.MediaType == "Image")
                    {
                        await _cloudinary.DestroyAsync(
                            new DeletionParams(media.FileName));
                    }
                    else
                    {
                        await _cloudinary.DestroyAsync(
                            new DeletionParams(media.FileName)
                            {
                                ResourceType = ResourceType.Video
                            });
                    }
                }
                catch (Exception ex)
                {
                    // DO NOT FAIL DELETE — just log
                    Console.WriteLine($"Cloudinary delete failed: {ex.Message}");
                }
            }
        }
        public async Task<bool> DeletePostAsync(int postId, int userId)
        {
            using var con = _dbFactory.CreateConnection();
            // Get media before delete
            var mediaList = await GetPostMediaAsync(postId);
            // Delete from Cloudinary
            await DeleteMediaFromCloudinaryAsync(mediaList);
            // Delete from DB
            var result = await con.QuerySingleAsync<int>(
                "sp_DeletePoste",
                new { PostId = postId, UserId = userId },
                commandType: CommandType.StoredProcedure
            );
            return result == 1;
        }

        public async Task<bool> UpdatePostStatusAsync(int postId, int userId, int status)
        {
            using var con = _dbFactory.CreateConnection();

            var result = await con.QuerySingleAsync<int>(
                "sp_UpdatePostActiveInActiveStatus",
                new
                {
                    PostId = postId,
                    UserId = userId,
                    Status = status
                },
                commandType: CommandType.StoredProcedure
            );

            return result == 1;
        }
    }
}
