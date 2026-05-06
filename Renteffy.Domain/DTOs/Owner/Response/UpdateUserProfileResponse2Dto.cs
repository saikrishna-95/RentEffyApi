using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.Owner.Response
{
    public class UpdateUserProfileResponse2Dto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        //public byte[]? ImageBytes { get; set; }
        public string? ImageUrl { get; set; }
    }
}
