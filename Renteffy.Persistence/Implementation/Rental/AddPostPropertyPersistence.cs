using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dapper;
using Microsoft.AspNetCore.Http;
using Renteffy.Domain.DTOs.Owner.Request;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.RequestDTOs;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.ResponseDTOs;
using Renteffy.Domain.Entities.Registration;
using Renteffy.Domain.Services.PersistanceInterfaces.Rental;
using Renteffy.Shared.Database.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Renteffy.Persistence.Implementation.Rental
{
    public class AddPostPropertyPersistence:IAddPostPropertyPersistence
    {
        private readonly IDbConnectionFactory _dbFactory;
        private readonly Cloudinary _cloudinary;

        public AddPostPropertyPersistence(IDbConnectionFactory dbFactory, Cloudinary cloudinary)
        {
            _dbFactory = dbFactory;
            _cloudinary = cloudinary;
        }

        public async Task<int> AddProperty(AddPropertyRequestDto request)
        {
            try 
            { 
            
            using var con = _dbFactory.CreateConnection();

            // ---------------------------------------
            // Amenities TVP
            // ---------------------------------------
            var amenitiesTable = new DataTable();
            amenitiesTable.Columns.Add("PropertyAmenityID",typeof(int));

            foreach (var amenity in request.Amenities)
            {
                amenitiesTable.Rows.Add(
                    amenity.PropertyAmenityID
                );
            }

            // ---------------------------------------
            // Rules TVP
            // ---------------------------------------
            var rulesTable = new DataTable();

            rulesTable.Columns.Add("RuleID",typeof(int));
            //rulesTable.Columns.Add("Active",typeof(int));

            foreach (var rule in request.Rules)
            {
                rulesTable.Rows.Add(
                    rule.RuleID
                );
            }

            // ---------------------------------------
            // Parameters
            // ---------------------------------------
            var parameters = new DynamicParameters();

            parameters.Add("@OwnerID", request.UserId);
            parameters.Add("@PropertyTypeID", request.PropertyTypeID);
            parameters.Add("@Mobile", request.Mobile);
            parameters.Add("@Latitude", request.Latitude);
            parameters.Add("@Longitude", request.Longitude);
            parameters.Add("@PropertyName", request.PropertyName);

            // Address
            parameters.Add("@HouseNo", request.HouseNo);
            parameters.Add("@Street", request.Street);
            parameters.Add("@Area", request.Area);
            parameters.Add("@City", request.City);
            parameters.Add("@State", request.State);
            parameters.Add("@PinCode", request.PinCode);
            parameters.Add("@FullAddress", request.FullAddress);
            parameters.Add("@PropertyConfigID", request.PropertyConfigID);
            parameters.Add("@Bedrooms", request.Bedrooms);
            parameters.Add("@Bathrooms", request.Bathrooms);
            parameters.Add("@Balconies", request.Balconies);
            parameters.Add("@FloorDetails", request.FloorDetails);
            parameters.Add("@RentalTypeID", request.RentalTypeID);
            parameters.Add("@PropertyFurnishID", request.PropertyFurnishID);
            parameters.Add("@MonthlyRentPrice", request.MonthlyRentPrice);
            parameters.Add("@SecurityDepositAmount", request.SecurityDepositAmount);
            parameters.Add("@MaintenanceID", request.MaintenanceID);
            parameters.Add("@AvailableFromDate",request.AvailableFromDate);
            parameters.Add("@PreferredTenantID",request.PreferredTenantID);
            parameters.Add("@AboutHome", request.AboutHome);
            parameters.Add("@Status", 1);
            parameters.Add("@Amenities",amenitiesTable.AsTableValuedParameter("dbo.PropertyAmenityList"));
            parameters.Add("@Rules",rulesTable.AsTableValuedParameter("dbo.PropertyRuleList"));

            // ---------------------------------------
            // Execute Property Stored Procedure
            // ---------------------------------------

            var propertyId = await con.QuerySingleAsync<int>("sp_AddProperty",parameters,commandType: CommandType.StoredProcedure);

            return propertyId;
            }
            catch
            {
                throw;
            }
        }

        public async Task SavePropertyMediaAsync(List<PropertyPostMediaDto> media)
        {
            using var con = _dbFactory.CreateConnection();

            await con.ExecuteAsync(@"
            INSERT INTO dbo.T_Property_PostMedia_TR
            (
                PropertyPostID,
                PropertyMediaTypeID,
                MediaType,
                FileName,
                FilePath,
                ContentType,
                CreatedBy
            )
            VALUES
            (
                @PropertyPostID,
                @PropertyMediaTypeID,
                @MediaType,
                @FileName,
                @FilePath,
                @ContentType,
                @CreatedBy
            )",
            media
           );
        }

        public async Task<int> UpdatePropertyAsync(UpdatePropertyRequestDto request,List<(int mediaId, IFormFile file)> replaceMedia,List<IFormFile> newFiles)
        {
            using var con = _dbFactory.CreateConnection();

            // ============================================
            // 1. BUILD AMENITIES TVP
            // ============================================

            var amenitiesTable = new DataTable();

            amenitiesTable.Columns.Add("PropertyAmenityID",typeof(int));

            foreach (var amenity in request.Amenities)
            {
                amenitiesTable.Rows.Add(amenity.PropertyAmenityID);
            }

            // ============================================
            // 2. BUILD RULES TVP
            // ============================================

            var rulesTable = new DataTable();

            rulesTable.Columns.Add("RuleID",typeof(int));

            foreach (var rule in request.Rules)
            {
                rulesTable.Rows.Add(rule.RuleID);
            }
            // ============================================
            // 3. UPDATE PROPERTY
            // ============================================

            var parameters = new DynamicParameters();

            parameters.Add("@PropertyID",request.PropertyID);
            parameters.Add("@OwnerID",request.UserId);
            parameters.Add("@PropertyTypeID",request.PropertyTypeID);
            parameters.Add("@Mobile",request.Mobile);
            parameters.Add("@Latitude",request.Latitude);
            parameters.Add("@Longitude",request.Longitude);
            parameters.Add("@PropertyName",request.PropertyName);
            parameters.Add("@HouseNo",request.HouseNo);
            parameters.Add("@Street",request.Street);
            parameters.Add("@Area",request.Area);
            parameters.Add("@City",request.City);
            parameters.Add("@State",request.State);
            parameters.Add("@PinCode",request.PinCode);
            parameters.Add("@FullAddress",request.FullAddress);
            parameters.Add("@PropertyConfigID",request.PropertyConfigID);
            parameters.Add("@Bedrooms",request.Bedrooms);
            parameters.Add("@Bathrooms",request.Bathrooms);
            parameters.Add("@Balconies",request.Balconies);
            parameters.Add("@FloorDetails",request.FloorDetails);
            parameters.Add("@RentalTypeID",request.RentalTypeID);
            parameters.Add("@PropertyFurnishID",request.PropertyFurnishID);
            parameters.Add("@MonthlyRentPrice",request.MonthlyRentPrice);
            parameters.Add("@SecurityDepositAmount",request.SecurityDepositAmount);
            parameters.Add("@MaintenanceID",request.MaintenanceID);
            parameters.Add("@AvailableFromDate",request.AvailableFromDate);
            parameters.Add("@PreferredTenantID",request.PreferredTenantID);
            parameters.Add("@AboutHome",request.AboutHome);
            parameters.Add("@Amenities",amenitiesTable.AsTableValuedParameter("dbo.PropertyAmenityList"));
            parameters.Add("@Rules",rulesTable.AsTableValuedParameter("dbo.PropertyRuleList"));

            var propertyId = await con.QuerySingleAsync<int>("sp_UpdateProperty",parameters,commandType: CommandType.StoredProcedure);

            // ============================================
            // 4. DELETE SELECTED MEDIA
            // ============================================

            await DeletePropertyMediaAsync(request.DeleteMediaIds);

            // ============================================
            // 5. REPLACE MEDIA
            // ============================================

            var replaced = await ReplacePropertyMediaAsync(propertyId,request.UserId,replaceMedia);
            await UpdatePropertyMediaAsync(replaced);

            // ============================================
            // 6. ADD NEW MEDIA
            // ============================================
            var added = await AddNewPropertyMediaAsync(propertyId,request.UserId, newFiles,request.Media);

            await SavePropertyMediaAsync(added);

            return propertyId;
        }

        private async Task DeletePropertyMediaAsync(List<int> mediaIds)
        {
            if (mediaIds == null || !mediaIds.Any())
                return;

            using var con = _dbFactory.CreateConnection();

            var mediaList = await GetPropertyMediaByIds(
                mediaIds);

            foreach (var media in mediaList)
            {
                await DeleteFromCloudinary(media);
            }

            await con.ExecuteAsync(@" DELETE FROM dbo.T_Property_PostMedia_TR WHERE PropertyPostMediaID IN @MediaIds ",new{MediaIds = mediaIds});
        }

        private async Task<List<PropertyMediaResponseDto>> GetPropertyMediaByIds(List<int> mediaIds)
        {
            using var con = _dbFactory.CreateConnection();

            var media = await con.QueryAsync<PropertyMediaResponseDto>(
                @"
                SELECT PropertyPostMediaID,
                        PropertyPostID,
                        PropertyMediaTypeID,
                        MediaType,
                        FileName,
                        FilePath,
                        ContentType
                FROM dbo.T_Property_PostMedia_TR
                WHERE PropertyPostMediaID IN @MediaIds ",new { MediaIds = mediaIds });

            return media.ToList();
        }

        private async Task<List<UpdatePropertyMediaDto>> ReplacePropertyMediaAsync(int propertyId,int userId,List<(int mediaId, IFormFile file)> replaceList)
        {
            var updatedList = new List<UpdatePropertyMediaDto>();

            if (replaceList == null ||
                !replaceList.Any())
            {
                return updatedList;
            }

            var ids = replaceList.Select(x => x.mediaId).ToList();

            var existingMedia = await GetPropertyMediaByIds(ids);

            foreach (var item in replaceList)
            {
                var oldMedia = existingMedia.FirstOrDefault(x => x.PropertyPostMediaID == item.mediaId);

                if (oldMedia == null)
                    continue;

                // ========================================
                // DELETE OLD CLOUDINARY FILE
                // ========================================

                await DeleteFromCloudinary(oldMedia);

                // ========================================
                // UPLOAD NEW FILE
                // ========================================

                await using var stream = item.file.OpenReadStream();
                RawUploadResult uploadResult;


                if (item.file.ContentType.StartsWith("video"))
                {
                    uploadResult = await _cloudinary.UploadAsync(
                            new VideoUploadParams
                            {
                                File = new FileDescription(item.file.FileName,stream),
                                Folder = $"properties/{propertyId}"
                            });
                }
                else
                {
                    uploadResult =
                        await _cloudinary.UploadAsync(
                            new ImageUploadParams
                            {
                                File = new FileDescription(item.file.FileName,stream),
                                Folder = $"properties/{propertyId}"
                            });
                }

                // ========================================
                // PREPARE UPDATED MEDIA
                // ========================================

                updatedList.Add(
                    new UpdatePropertyMediaDto
                    {
                        PropertyPostMediaID = oldMedia.PropertyPostMediaID,
                        PropertyPostID = propertyId,
                        PropertyMediaTypeID = oldMedia.PropertyMediaTypeID,
                        MediaType = item.file.ContentType.StartsWith("video") ? "Video" : "Photo",
                        FileName = uploadResult.PublicId,
                        FilePath = uploadResult.SecureUrl.ToString(),
                        ContentType = item.file.ContentType,
                        UpdatedBy = userId
                    });
            }

            return updatedList;
        }

        public async Task UpdatePropertyMediaAsync( List<UpdatePropertyMediaDto> mediaList)
        {
            if (mediaList == null ||
                !mediaList.Any())
            {
                return;
            }

            using var con = _dbFactory.CreateConnection();

            foreach (var media in mediaList)
            {
                await con.ExecuteAsync(
                    @"
                UPDATE dbo.T_Property_PostMedia_TR
                SET PropertyMediaTypeID = @PropertyMediaTypeID,
                    MediaType = @MediaType,
                    FileName = @FileName,
                    FilePath = @FilePath,
                    ContentType = @ContentType,
                    UpdatedDate = CAST(GETDATE() AS DATE),
                    UpdatedBy = @UpdatedBy
              WHERE PropertyPostMediaID = @PropertyPostMediaID ",media);
            }
        }

        private async Task<List<PropertyMediaDto>> AddNewPropertyMediaAsync(int propertyId, int userId, List<IFormFile> files,List<PropertyMediaDto> mediaMeta)
        {
            var mediaList = new List<PropertyMediaDto>();

            if (files == null ||
                !files.Any())
            {
                return mediaList;
            }

            foreach (var file in files)
            {
                var meta = mediaMeta?.FirstOrDefault(x => x.FileName == file.FileName);

                if (meta == null)
                    continue;

                await using var stream = file.OpenReadStream();
                RawUploadResult uploadResult;

                if (file.ContentType.StartsWith("video"))
                {
                    uploadResult = await _cloudinary.UploadAsync(
                            new VideoUploadParams
                            {
                                File = new FileDescription(file.FileName,stream),
                                Folder = $"properties/{propertyId}"
                            });
                }
                else
                {
                    uploadResult = await _cloudinary.UploadAsync(
                            new ImageUploadParams
                            {
                                File = new FileDescription(file.FileName,stream),
                                Folder = $"properties/{propertyId}"
                            });
                }


                mediaList.Add(
                    new PropertyMediaDto
                    {
                        PropertyPostID = propertyId,
                        PropertyMediaTypeID = meta.PropertyMediaTypeID,
                        MediaType = file.ContentType.StartsWith("video") ? "Video" : "Photo",
                        FileName = uploadResult.PublicId,
                        FilePath = uploadResult.SecureUrl.ToString(),
                        ContentType = file.ContentType
                    });
            }

            return mediaList;
        }

        public async Task SavePropertyMediaAsync(List<PropertyMediaDto> media)
        {
            if (media == null ||
                !media.Any())
            {
                return;
            }

            using var con = _dbFactory.CreateConnection();
            await con.ExecuteAsync(
                @"
                INSERT INTO dbo.T_Property_PostMedia_TR
                (
                    PropertyPostID,
                    PropertyMediaTypeID,
                    MediaType,
                    FileName,
                    FilePath,
                    ContentType,
                    CreatedBy
                )
                VALUES
                (
                    @PropertyPostID,
                    @PropertyMediaTypeID,
                    @MediaType,
                    @FileName,
                    @FilePath,
                    @ContentType,
                    @CreatedBy
                )
                ",media);
        }

        private async Task<List<PropertyMediaResponseDto>> GetPropertyMediaAsync(int propertyId)
        {
            using var con = _dbFactory.CreateConnection();

            var media = await con.QueryAsync<PropertyMediaResponseDto>(
                @" SELECT   PropertyPostMediaID,
                            PropertyPostID,
                            PropertyMediaTypeID,
                            MediaType,
                            FileName,
                            FilePath,
                            ContentType
                    FROM    dbo.T_Property_PostMedia_TR
                    WHERE   PropertyPostID = @PropertyID
                    ",new{PropertyID = propertyId});

            return media.ToList();
        }

        private async Task DeletePropertyMediaFromCloudinaryAsync(List<PropertyMediaResponseDto> mediaList)
        {
            if (mediaList == null ||
                !mediaList.Any())
            {
                return;
            }

            foreach (var media in mediaList)
            {
                await DeleteFromCloudinary(media);
            }
        }

        private async Task DeleteFromCloudinary(PropertyMediaResponseDto media)
        {
            if (string.IsNullOrEmpty(media.FileName))  return;
            try
            {
                var resourceType = media.ContentType != null && media.ContentType.StartsWith("video") ? ResourceType.Video : ResourceType.Image;
                await _cloudinary.DestroyAsync(new DeletionParams(media.FileName){ResourceType = resourceType});
            }
            catch
            {
                // Log error if required
            }
        }

        public async Task<bool> DeletePropertyAsync(int propertyId,int userId)
        {
            using var con = _dbFactory.CreateConnection();

            // ============================================
            // 1. GET PROPERTY MEDIA BEFORE DELETE
            // ============================================

            var mediaList = await GetPropertyMediaAsync(
                propertyId);

            // ============================================
            // 2. DELETE MEDIA FROM CLOUDINARY
            // ============================================

            await DeletePropertyMediaFromCloudinaryAsync(mediaList);

            // ============================================
            // 3. DELETE PROPERTY FROM DATABASE
            // ============================================

            var result = await con.QuerySingleAsync<int>("sp_DeleteProperty",
                new
                {
                    PropertyID = propertyId,
                    UserID = userId
                },
                commandType: CommandType.StoredProcedure
            );
            return result == 1;
        }

        public async Task<bool> UpdatePropertyStatusAsync(int propertyId,int userId,int status)
        {
            using var con = _dbFactory.CreateConnection();

            var result = await con.QuerySingleAsync<int>("sp_UpdatePropertyActiveInActiveStatus",
                new
                {
                    PropertyID = propertyId,
                    UserID = userId,
                    Status = status
                },
                commandType: CommandType.StoredProcedure
            );

            return result == 1;
        }
    }
}
