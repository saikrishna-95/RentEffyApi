using System;
using System.Collections.Generic;
using System.Text;

namespace Renteffy.Domain.DTOs.UserTrans.Response
{
    public class MediaCategoryResponseDTO
    {
        public int MediaCategoryId { get; set; }
        public string CategoryName { get; set; }
        public int MaxFiles { get; set; }
    }
}
