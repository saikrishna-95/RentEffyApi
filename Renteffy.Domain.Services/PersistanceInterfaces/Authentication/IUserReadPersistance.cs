using Renteffy.Domain.DTOs.Owner.Response;
using Renteffy.Domain.Entities.Registration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.PersistanceInterfaces.Authentication
{
    public interface IUserReadPersistance
    {
        Task<Users?> GetUserByUserIdAsync(int userid);
        Task<Users?> GetUserByUserNameAsync(string userName);

        Task<List<string>> GetUserRolesAsync(int userId);
        Task<List<string>> GetUserPermissionsAsync(int userId);

        Task<UserProfileResponseDto> GetUserProfile(int userId);

        Task<UpdateUserProfileResponse2Dto> UpdateUserProfile(UpdateUserProfileResponseDto model);
    }
}
