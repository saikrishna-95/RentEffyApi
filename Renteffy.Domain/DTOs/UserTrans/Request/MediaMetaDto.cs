using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Request
{
    public class MediaMetaDto
    {
        public string FileName { get; set; }
        public int MediaCategoryId { get; set; }
    }
}
