using Dapper;
using Renteffy.Domain.DTOs.RentalDTOs.Transactions.ResponseDTOs;
using Renteffy.Domain.Services.PersistanceInterfaces.Rental;
using Renteffy.Shared.Database.DbConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Renteffy.Persistence.Implementation.Rental
{
    public class GetAllPropertiesPersistance : IGetAllPropertiesPersistance
    {
        private readonly IDbConnectionFactory _dbFactory;

        public GetAllPropertiesPersistance(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<PublicPostPropertiesResponseDto>> GetPublicPostPropertiesAsync()
        {
            using var con = _dbFactory.CreateConnection();

            using var multi = await con.QueryMultipleAsync("sp_GetPublicProperties", commandType: CommandType.StoredProcedure);

            // 1. Main Property
            var properties = (await multi.ReadAsync<PublicPostPropertiesResponseDto>()).ToList();

            // 2. Amenities
            var amenities = (await multi.ReadAsync<PropertyAmenitiesResponseDto>()).ToList();

            // 3. Rules
            var rules = (await multi.ReadAsync<PropertyRulesResponseDto>()).ToList();

            // 4. Media
            var media = (await multi.ReadAsync<PropertyMediaResponseDto>()).ToList();

            // ---------------------------------------
            // Map child records to Property
            // ---------------------------------------

            foreach (var property in properties)
            {
                property.Amenities = amenities.Where(a => a.PropertyPostID == property.PropertyID).ToList();
                property.Rules = rules.Where(r => r.PropertyPostID == property.PropertyID).ToList();
                property.Media = media.Where(m => m.PropertyPostID == property.PropertyID).ToList();
            }

            return properties;
        }

        public async Task<List<PublicPostPropertiesResponseDto>> GetPropertyByIdAsync(int propertyid)
        {
            using var con = _dbFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@PropertyID", propertyid);

            using var multi = await con.QueryMultipleAsync("sp_GetPropertyById", parameters,commandType: CommandType.StoredProcedure);

            var properties = (await multi.ReadAsync<PublicPostPropertiesResponseDto>()).ToList();

            var amenities = (await multi.ReadAsync<PropertyAmenitiesResponseDto>()).ToList();

            var rules = (await multi.ReadAsync<PropertyRulesResponseDto>()).ToList();

            var media = (await multi.ReadAsync<PropertyMediaResponseDto>()).ToList();

            foreach (var property in properties)
            {
                property.Amenities = amenities.Where(x => x.PropertyPostID == property.PropertyID).ToList();
                property.Rules = rules.Where(x => x.PropertyPostID == property.PropertyID).ToList();
                property.Media = media.Where(x => x.PropertyPostID == property.PropertyID).ToList();
            }
            return properties;
        }

        public async Task<List<PublicPostPropertiesResponseDto>> GetPropertiesByOwnerIdAsync(int ownerId)
        {
            using var con = _dbFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@OwnerID", ownerId);

            using var multi = await con.QueryMultipleAsync("sp_GetPropertiesByOwnerId", parameters,commandType: CommandType.StoredProcedure);

            // 1. Properties
            var properties = (await multi.ReadAsync<PublicPostPropertiesResponseDto>()).ToList();

            // 2. Amenities
            var amenities = (await multi.ReadAsync<PropertyAmenitiesResponseDto>()).ToList();

            // 3. Rules
            var rules = (await multi.ReadAsync<PropertyRulesResponseDto>()).ToList();

            // 4. Media
            var media = (await multi.ReadAsync<PropertyMediaResponseDto>()).ToList();

            // Map child data to each property
            foreach (var property in properties)
            {
                property.Amenities = amenities.Where(x => x.PropertyPostID == property.PropertyID).ToList();
                property.Rules = rules.Where(x => x.PropertyPostID == property.PropertyID).ToList();
                property.Media = media.Where(x => x.PropertyPostID == property.PropertyID).ToList();
            }

            return properties;
        }

        public async Task<EditPropertyResponseDto> GetPropertyForEditAsync(int propertyId)
        {
            using var connection = _dbFactory.CreateConnection();

            using var multi = await connection.QueryMultipleAsync("sp_GetPropertyForEdit",new { PropertyID = propertyId },commandType: CommandType.StoredProcedure);

            var property = await multi.ReadFirstOrDefaultAsync<EditPropertyResponseDto>();

            if (property == null)
                return null;

            property.Media = (await multi.ReadAsync<PropertyMediaResponseDto>()).ToList();
            property.Amenities = (await multi.ReadAsync<PropertyAmenitiesResponseDto>()).ToList();
            property.Rules = (await multi.ReadAsync<PropertyRulesResponseDto>()).ToList();

            return property;
        }

    }
}
