using Dapper;
using Microsoft.AspNetCore.Identity;
using Renteffy.Application.Interfaces.Registration;
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

        public UserReadPersistance(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
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
    }
}
