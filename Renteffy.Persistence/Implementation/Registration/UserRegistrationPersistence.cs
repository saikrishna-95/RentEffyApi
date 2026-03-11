using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Renteffy.Domain.Entities.Registration;
using Renteffy.Domain.Services.PersistanceInterfaces;
using Renteffy.Persistence.RegistrationDbContext;
using Renteffy.Shared.Database.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Renteffy.Persistence.Implementation.Registration
{
    public class UserRegistrationPersistence:IUserRegistrationPersistence
    {
        //private readonly AppDbContext _db;
        private readonly IDbConnectionFactory _dbFactory;
        public UserRegistrationPersistence(IDbConnectionFactory dbFactory) //AppDbContext db,
        { 
            //_db = db;
            _dbFactory = dbFactory;
        }
        public async Task<int> RegisterUserAsync(Users user)
        {
            using var con = _dbFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@FullName", user.FullName);
            parameters.Add("@Email", user.Email);
            parameters.Add("@PasswordHash", user.PasswordHash);
            parameters.Add("@Mobile", user.Mobile);
            //parameters.Add("@IsDeleted", user.IsDeleted);

            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await con.ExecuteAsync(
                "sp_RegisterUser",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@Result");
        }
    }
}
