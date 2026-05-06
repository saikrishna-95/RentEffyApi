using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Renteffy.Application.Interfaces.Registration;
using Renteffy.Domain.DTOs.Owner.Response;
using Renteffy.Domain.Entities.Registration;
using Renteffy.Domain.Services.PersistanceInterfaces.Authentication;
using Renteffy.Shared.Database.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Renteffy.Persistence.Implementation.Authentication
{
    public class UserReadPersistance:IUserReadPersistance
    {
        private readonly IDbConnectionFactory _dbFactory;
        private readonly Cloudinary _cloudinary;

        public UserReadPersistance(IDbConnectionFactory dbFactory, Cloudinary cloudinary)
        {
            _dbFactory = dbFactory;
            _cloudinary = cloudinary;
        }

        public async Task<Users?> GetUserByUserIdAsync(int userId)
        {
            using var con = _dbFactory.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Users>("sp_GetUserById",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Users?> GetUserByUserNameAsync(string userName)
        {
            using var con = _dbFactory.CreateConnection();
            return await con.QueryFirstOrDefaultAsync<Users>("sp_LoginUser",
                new { Email = userName },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            using var con = _dbFactory.CreateConnection();
            var roles = await con.QueryAsync<string>("sp_GetUserRoles",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );
            return roles.ToList();
        }

        public async Task<List<string>> GetUserPermissionsAsync(int userId)
        {
            using var con = _dbFactory.CreateConnection();
            var permissions = await con.QueryAsync<string>("sp_GetUserPermissions",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );
            return permissions.ToList();
        }

        public async Task<UserProfileResponseDto> GetUserProfile(int userId)
        {
            using var con = _dbFactory.CreateConnection();

            var profile = await con.QueryFirstOrDefaultAsync<UserProfileResponseDto>(
                "sp_GetUserProfile",
                new { UserId = userId },
                commandType: CommandType.StoredProcedure
            );
            return profile;
        }

        private async Task<string?> UploadProfileImageAsync(int userId, IFormFile file)
        {
            if (file == null) return null;

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = $"users/{userId}/profile",
                Overwrite = true,     // replace old image
                Invalidate = true     // clear CDN cache
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            return result.SecureUrl.ToString();
        }

        public async Task<UpdateUserProfileResponse2Dto> UpdateUserProfile(UpdateUserProfileResponseDto model)
        {
            using var con = _dbFactory.CreateConnection();
            string? imageUrl = null;

            if (model.Image != null)
            {
                imageUrl = await UploadProfileImageAsync(model.UserId, model.Image);
            }
            
            var updatedprofile = await con.QueryFirstOrDefaultAsync<UpdateUserProfileResponse2Dto>(
                "sp_UpdateUserProfile",
                new
                {
                    model.UserId,
                    model.FullName,
                    model.Email,
                    model.Mobile,
                    ImageUrl = imageUrl
                },
                commandType: CommandType.StoredProcedure
            );
            return updatedprofile;
        }

    }
}
