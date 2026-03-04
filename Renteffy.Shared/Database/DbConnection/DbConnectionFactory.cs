using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Renteffy.Shared.Database.DbConnection
{
    public class DbConnectionFactory:IDbConnectionFactory
    {
        private readonly IConfiguration _config;

        public DbConnectionFactory(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection CreateConnection()
        {
            var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            con.Open();
            return con;
        }
    }
}
