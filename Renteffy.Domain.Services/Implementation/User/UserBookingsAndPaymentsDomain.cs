using Microsoft.Extensions.Configuration;
using Renteffy.Domain.DTOs.UserTrans.Request;
using Renteffy.Domain.DTOs.UserTrans.Response;
using Renteffy.Domain.Services.Interfaces.User;
using Renteffy.Domain.Services.PersistanceInterfaces.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.Services.Implementation.User
{
    public class UserBookingsAndPaymentsDomain:IUserBookingsAndPaymentsDomain
    {
        private readonly IUserBookingsAndPaymentsPersistance _readRepo;
        private readonly IConfiguration _config;

        public UserBookingsAndPaymentsDomain(IUserBookingsAndPaymentsPersistance readRepo, IConfiguration config)
        {
            _readRepo = readRepo;
            _config = config;
        }

        public async Task<int> CreateBookingAsync(CreateBookingRequestDTO request) 
            => await _readRepo.CreateBookingAsync(request);

        public async Task<int> ConfirmBookingAsync(ConfirmBookingRequestDTO confirm)
            => await _readRepo.ConfirmBookingAsync(confirm);

        public async Task<int> CancelBookingAsync(CancelBookingRequestDTO cancel)
            => await _readRepo.CancelBookingAsync(cancel);
    }
}
