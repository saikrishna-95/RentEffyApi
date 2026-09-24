using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Renteffy.Shared.Database.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Renteffy.Integration.Services.jobs
{
    public class PropertyBookingExpiryService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public PropertyBookingExpiryService(
            IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope =_scopeFactory.CreateScope();
                    var dbFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
                    using var con = dbFactory.CreateConnection();
                    await con.ExecuteAsync("sp_Property_ExpirePendingBookings",commandType:CommandType.StoredProcedure);
                }
                catch
                {
                    // Add your existing logger here.
                }

                await Task.Delay(TimeSpan.FromMinutes(1),stoppingToken);
            }
        }
    }
}
